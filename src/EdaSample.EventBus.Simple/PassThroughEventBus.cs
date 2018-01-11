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
    public sealed class PassThroughEventBus : IEventBus
    {
        private readonly EventQueue eventQueue = new EventQueue();
        private readonly ILogger logger;
        private readonly IEventHandlerExecutionContext context;

        public PassThroughEventBus(IEventHandlerExecutionContext context,
            ILogger<PassThroughEventBus> logger)
        {
            this.context = context;
            this.logger = logger;
            logger.LogInformation($"PassThroughEventBus构造函数调用完成。Hash Code：{this.GetHashCode()}.");

            eventQueue.EventPushed += EventQueue_EventPushed;
        }

        private async void EventQueue_EventPushed(object sender, EventProcessedEventArgs e)
            => await this.context.HandleEventAsync(e.Event);

        public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent
                => Task.Factory.StartNew(() => eventQueue.Push(@event));

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            if (!this.context.HandlerRegistered<TEvent, TEventHandler>())
            {
                this.context.RegisterHandler<TEvent, TEventHandler>();
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        void Dispose(bool disposing)
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
        public void Dispose() => Dispose(true);

        #endregion
    }
}
