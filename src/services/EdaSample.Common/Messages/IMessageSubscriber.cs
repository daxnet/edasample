using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Messages
{
    public interface IMessageSubscriber<in TBaseMessageType, in TBaseMessageHandlerType>
        where TBaseMessageType : IMessage
        where TBaseMessageHandlerType : IMessageHandler
    {
        void Subscribe<TMessage, TMessageHandler>()
            where TMessage : TBaseMessageType
            where TMessageHandler : TBaseMessageHandlerType;

        void Subscribe(Type messageType, Type messageHandlerType);
    }
}
