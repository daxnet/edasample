using EdaSample.Common.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Events
{
    public abstract class BaseEventBus : IEventBus
    {
        protected readonly IMessageHandlerContext messageHandlerContext;

        protected BaseEventBus(IMessageHandlerContext messageHandlerContext)
        {
            this.messageHandlerContext = messageHandlerContext;
        }

        Task IMessagePublisher.PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken) => this.PublishEventAsync(message as IEvent);



        public abstract void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public abstract Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;

        #endregion

    }
}
