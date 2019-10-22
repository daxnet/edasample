using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas
{
    public interface IStartWith<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {

    }
}
