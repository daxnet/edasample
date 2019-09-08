using EdaSample.Common.Events;
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
    public abstract class RabbitMQMessageBus<TBaseMessageType, TBaseMessageHandlerType> : MessageBus<TBaseMessageType, TBaseMessageHandlerType>
        where TBaseMessageType : IMessage
        where TBaseMessageHandlerType : IMessageHandler
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string exchangeName;
        private readonly string exchangeType;
        private readonly string queueName;
        private readonly bool autoAck;
        private readonly ILogger logger;
        private bool disposed;

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
    }
}