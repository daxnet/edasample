using System;

namespace EdaSample.Common.Messages
{
    public class MessageProcessedEventArgs : EventArgs
    {
        public MessageProcessedEventArgs(IMessage message)
        {
            this.Message = message;
        }

        public IMessage Message { get; }
    }
}