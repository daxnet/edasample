using EdaSample.Common.Events;
using EdaSample.Common.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.EventBus.Simple
{
    internal sealed class MessageQueue
    {
        public event System.EventHandler<MessageProcessedEventArgs> MessagePushed;

        public MessageQueue() { }

        public void Push(IMessage message)
        {
            OnMessagePushed(new MessageProcessedEventArgs(message));
        }

        private void OnMessagePushed(MessageProcessedEventArgs e) => this.MessagePushed?.Invoke(this, e);
    }
}
