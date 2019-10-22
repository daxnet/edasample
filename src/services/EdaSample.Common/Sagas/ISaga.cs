using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas
{
    public interface ISaga
    {
    }

    public interface ISaga<TState, TEvent> : IStartWith<TEvent>
        where TState : ISagaState
        where TEvent : IEvent
    { }
}
