using EdaSample.Common.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.EventBus.Simple
{
    internal sealed class EventQueue
    {
        public event System.EventHandler<EventProcessedEventArgs> EventPushed;

        public EventQueue() { }

        public void Push(IEvent @event)
        {
            OnMessagePushed(new EventProcessedEventArgs(@event));
        }

        private void OnMessagePushed(EventProcessedEventArgs e) => this.EventPushed?.Invoke(this, e);
    }
}
