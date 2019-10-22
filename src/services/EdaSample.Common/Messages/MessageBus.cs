using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Messages
{
    public abstract class MessageBus<TBaseMessageType, TBaseMessageHandlerType> : IMessageBus<TBaseMessageType, TBaseMessageHandlerType>
        where TBaseMessageType : IMessage
        where TBaseMessageHandlerType : IMessageHandler
    {
        protected readonly IMessageHandlerContext messageHandlerContext;

        ~MessageBus()
        {
            Dispose(false);
        }

        protected MessageBus(IMessageHandlerContext messageHandlerContext)
            => this.messageHandlerContext = messageHandlerContext;

        protected virtual void Dispose(bool disposing) { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : TBaseMessageType;

        public abstract void Subscribe<TMessage, TMessageHandler>()
            where TMessage : TBaseMessageType
            where TMessageHandler : TBaseMessageHandlerType;

        public abstract void Subscribe(Type messageType, Type messageHandlerType);
    }
}
