// ============================================================================
//   ______    _        _____                       _
//  |  ____|  | |      / ____|                     | |
//  | |__   __| | __ _| (___   __ _ _ __ ___  _ __ | | ___
//  |  __| / _` |/ _` |\___ \ / _` | '_ ` _ \| '_ \| |/ _ \
//  | |___| (_| | (_| |____) | (_| | | | | | | |_) | |  __/
//  |______\__,_|\__,_|_____/ \__,_|_| |_| |_| .__/|_|\___|
//                                           | |
//                                           |_|
// MIT License
//
// Copyright (c) 2017-2019 Sunny Chen (daxnet)
//
// ============================================================================

using EdaSample.Common.Messages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Messaging.RabbitMQ
{
    /// <summary>
    /// Represents the base class for the message buses that utilize the RabbitMQ as the back-end messaging mechanism.
    /// </summary>
    /// <typeparam name="TBaseMessageType">The type of the base message type.</typeparam>
    /// <typeparam name="TBaseMessageHandlerType">The type of the base message handler type.</typeparam>
    /// <seealso cref="EdaSample.Common.Messages.MessageBus{TBaseMessageType, TBaseMessageHandlerType}" />
    public abstract class RabbitMQMessageBus<TBaseMessageType, TBaseMessageHandlerType> : MessageBus<TBaseMessageType, TBaseMessageHandlerType>
        where TBaseMessageType : IMessage
        where TBaseMessageHandlerType : IMessageHandler
    {
        #region Private Fields

        private readonly bool autoAck;
        private readonly IModel channel;
        private readonly IConnection connection;
        private readonly IConnectionFactory connectionFactory;
        private readonly string exchangeName;
        private readonly string exchangeType;
        private readonly ILogger logger;
        private readonly string queueName;
        private bool disposed;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQMessageBus{TBaseMessageType, TBaseMessageHandlerType}"/> class.
        /// </summary>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="context">The context.</param>
        /// <param name="exchangeName">Name of the exchange.</param>
        /// <param name="exchangeType">Type of the exchange.</param>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="autoAck">if set to <c>true</c> [automatic ack].</param>
        public RabbitMQMessageBus(IConnectionFactory connectionFactory,
            ILogger<RabbitMQMessageBus<TBaseMessageType, TBaseMessageHandlerType>> logger,
            IMessageHandlerContext context,
            string exchangeName,
            string exchangeType = ExchangeType.Fanout,
            string queueName = null,
            bool autoAck = false)
            : base(context)
        {
            this.connectionFactory = connectionFactory;
            this.logger = logger;
            this.connection = this.connectionFactory.CreateConnection();
            this.channel = this.connection.CreateModel();
            this.exchangeType = exchangeType;
            this.exchangeName = exchangeName;
            this.autoAck = autoAck;

            this.channel.ExchangeDeclare(this.exchangeName, this.exchangeType);

            this.queueName = this.InitializeEventConsumer(queueName);

            logger.LogInformation($"RabbitMQEventBus构造函数调用完成。Hash Code：{this.GetHashCode()}.");
        }

        #endregion Public Constructors

        #region Public Methods

        public override Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
        {
            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var eventBody = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(this.exchangeName,
                message.GetType().FullName,
                null,
                eventBody);
            return Task.CompletedTask;
        }

        public override void Subscribe<TMessage, TMessageHandler>()
        {
            if (!this.messageHandlerContext.HandlerRegistered<TMessage, TMessageHandler>())
            {
                this.messageHandlerContext.RegisterHandler<TMessage, TMessageHandler>();
                this.channel.QueueBind(this.queueName, this.exchangeName, typeof(TMessage).FullName);
            }
        }

        public override void Subscribe(Type messageType, Type messageHandlerType)
        {
            if (!this.messageHandlerContext.HandlerRegistered(messageType, messageHandlerType))
            {
                this.messageHandlerContext.RegisterHandler(messageType, messageHandlerType);
                this.channel.QueueBind(this.queueName, this.exchangeName, messageType.FullName);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.channel.Dispose();
                    this.connection.Dispose();

                    logger.LogInformation($"RabbitMQEventBus已经被Dispose。Hash Code:{this.GetHashCode()}.");
                }

                disposed = true;
                base.Dispose(disposing);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private string InitializeEventConsumer(string queue)
        {
            var localQueueName = queue;
            if (string.IsNullOrEmpty(localQueueName))
            {
                localQueueName = this.channel.QueueDeclare().QueueName;
            }
            else
            {
                this.channel.QueueDeclare(localQueueName, true, false, false, null);
            }

            var consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += async (model, eventArgument) =>
            {
                var messageBody = eventArgument.Body;
                var json = Encoding.UTF8.GetString(messageBody);
                var message = (IMessage)JsonConvert.DeserializeObject(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                try
                {
                    await this.messageHandlerContext.HandleMessageAsync(message);
                    if (!autoAck)
                    {
                        channel.BasicAck(eventArgument.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "事件处理器执行失败。");
                }
            };

            this.channel.BasicConsume(localQueueName, autoAck: this.autoAck, consumer: consumer);

            return localQueueName;
        }

        #endregion Private Methods
    }
}