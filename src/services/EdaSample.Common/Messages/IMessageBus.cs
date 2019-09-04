using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Messages
{
    public interface IMessageBus : IMessagePublisher, IMessageSubscriber
    {
    }
}
