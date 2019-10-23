using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas
{
    public interface ISaga<TState, TEvent> : IStartWith<TEvent>, IAggregateRoot
        where TState : ISagaState
        where TEvent : IEvent
    { }
}
