using EdaSample.Common.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.EventBus.Simple
{
    public sealed class PassThroughEventBus : BaseEventBus
    {
        private readonly EventQueue eventQueue = new EventQueue();
        private readonly ILogger logger;

        public PassThroughEventBus(IEventHandlerExecutionContext context,
            ILogger<PassThroughEventBus> logger)
            : base(context)
        {
            this.logger = logger;
            logger.LogInformation($"PassThroughEventBus构造函数调用完成。Hash Code：{this.GetHashCode()}.");

            eventQueue.EventPushed += EventQueue_EventPushed;
        }

        private async void EventQueue_EventPushed(object sender, EventProcessedEventArgs e)
            => await this.eventHandlerExecutionContext.HandleEventAsync(e.Event);

        public override Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() => eventQueue.Push(@event));
        }

        public override void Subscribe<TEvent, TEventHandler>()
        {
            if (!this.eventHandlerExecutionContext.HandlerRegistered<TEvent, TEventHandler>())
            {
                this.eventHandlerExecutionContext.RegisterHandler<TEvent, TEventHandler>();
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
                    this.eventQueue.EventPushed -= EventQueue_EventPushed;
                    logger.LogInformation($"PassThroughEventBus已经被Dispose。Hash Code:{this.GetHashCode()}.");
                }

                disposedValue = true;
            }
        }

        #endregion
    }
}
