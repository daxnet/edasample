using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas
{
    public interface ISagaManager
    {
        void Register<TState, TEvent, TSaga>()
            where TState : ISagaState
            where TEvent : IEvent
            where TSaga : ISaga<TState, TEvent>;
    }
}
