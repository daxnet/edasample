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

using EdaSample.Common.Events;
using EdaSample.Common.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Messaging.Simple
{
    public sealed class PassThroughEventBus : IEventBus
    {
        #region Private Fields

        private readonly ILogger logger;
        private readonly IMessageHandlerContext messageHandlerContext;
        private readonly MessageQueue messageQueue = new MessageQueue();
        private bool disposedValue;

        #endregion Private Fields

        #region Public Constructors

        public PassThroughEventBus(IMessageHandlerContext messageHandlerContext,
                    ILogger<PassThroughEventBus> logger)
        {
            this.messageHandlerContext = messageHandlerContext;
            this.logger = logger;
            logger.LogInformation($"PassThroughEventBus构造函数调用完成。Hash Code：{this.GetHashCode()}.");

            messageQueue.MessagePushed += MessageQueue_MessagePushed;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
        }

        public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : IEvent
        {
            return Task.Factory.StartNew(() =>
            {
                messageQueue.Push(message);
            });
        }

        public void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IEvent
            where TMessageHandler : IEventHandler
        {
            if (!this.messageHandlerContext.HandlerRegistered<TMessage, TMessageHandler>())
            {
                this.messageHandlerContext.RegisterHandler<TMessage, TMessageHandler>();
            }
        }

        public void Subscribe(Type messageType, Type messageHandlerType)
        {
            if (!this.messageHandlerContext.HandlerRegistered(messageType, messageHandlerType))
            {
                this.messageHandlerContext.RegisterHandler(messageType, messageHandlerType);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.messageQueue.MessagePushed -= MessageQueue_MessagePushed;
                    logger.LogInformation($"PassThroughEventBus已经被Dispose。Hash Code:{this.GetHashCode()}.");
                }

                disposedValue = true;
            }
        }

        private async void MessageQueue_MessagePushed(object sender, MessageProcessedEventArgs e) => await this.messageHandlerContext.HandleMessageAsync(e.Message);

        #endregion Private Methods
    }
}