using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdaSample.Common.Events;

namespace EdaSample.Common.Sagas
{
    public class SagaManager : ISagaManager
    {
        private readonly IEventBus eventBus;

        public SagaManager(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public void Register<TState, TEvent, TSaga>()
            where TState : ISagaState
            where TEvent : IEvent
            where TSaga : ISaga<TState, TEvent>
        {
            this.eventBus.Subscribe(typeof(TEvent), typeof(TSaga));
            var eventTypes = typeof(TSaga).GetInterfaces()
                .Where(i => i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICanHandle<>))
                .Select(i => i.GetGenericArguments()[0]);
            foreach(var et in eventTypes)
            {
                this.eventBus.Subscribe(et, typeof(TSaga));
            }
        }
    }
}
