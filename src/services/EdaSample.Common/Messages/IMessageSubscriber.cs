using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Messages
{
    public interface IMessageSubscriber
    {
        void Subscribe<TMessage, TMessageHandler>()
            where TMessage : IMessage
            where TMessageHandler : IMessageHandler<TMessage>;
    }
}
