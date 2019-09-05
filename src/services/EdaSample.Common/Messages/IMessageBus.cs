using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Messages
{
    public interface IMessageBus<in TBaseMessageType, in TBaseMessageHandlerType> : IMessagePublisher<TBaseMessageType>, IMessageSubscriber<TBaseMessageType, TBaseMessageHandlerType>
        where TBaseMessageType : IMessage
        where TBaseMessageHandlerType : IMessageHandler
    {
    }
}
