using EdaSample.Common.Events;
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
        private readonly IEnumerable<IEventHandler> eventHandlers;

        public PassThroughEventBus(IEnumerable<IEventHandler> eventHandlers)
        {
            this.eventHandlers = eventHandlers;
        }

        private void EventQueue_EventPushed(object sender, EventProcessedEventArgs e)
            => (from eh in this.eventHandlers
                where eh.CanHandle(e.Event)
                select eh).ToList().ForEach(async eh => await eh.HandleAsync(e.Event));

        public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent
                => Task.Factory.StartNew(() => eventQueue.Push(@event));

        public void Subscribe()
            => eventQueue.EventPushed += EventQueue_EventPushed;


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.eventQueue.EventPushed -= EventQueue_EventPushed;
                }

                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
