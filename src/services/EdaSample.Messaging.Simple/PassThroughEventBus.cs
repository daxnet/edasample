using EdaSample.Common.Events;
using EdaSample.Common.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Messaging.Simple
{
    public sealed class PassThroughEventBus : BaseEventBus
    {
        private readonly MessageQueue messageQueue = new MessageQueue();
        private readonly ILogger logger;

        public PassThroughEventBus(IMessageHandlerContext context,
            ILogger<PassThroughEventBus> logger)
            : base(context)
        {
            this.logger = logger;
            logger.LogInformation($"PassThroughEventBus构造函数调用完成。Hash Code：{this.GetHashCode()}.");

            messageQueue.MessagePushed += MessageQueue_MessagePushed;
        }

        private async void MessageQueue_MessagePushed(object sender, MessageProcessedEventArgs e)
            => await this.messageHandlerContext.HandleMessageAsync(e.Message);

        public override Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                messageQueue.Push(@event);
            });
        }

        public override void Subscribe<TEvent, TEventHandler>()
        {
            if (!this.messageHandlerContext.HandlerRegistered<TEvent, TEventHandler>())
            {
                this.messageHandlerContext.RegisterHandler<TEvent, TEventHandler>();
            }
        }

        #region IDisposable Support

        private bool disposedValue;

        protected override void Dispose(bool disposing)
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

        #endregion
    }
}
