using EdaSample.Common.Commands;
using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.Sagas
{
    public abstract class Saga<TState> where TState : ISagaState
    {
        protected readonly ICommandBus commandBus;
        protected readonly IEventBus eventBus;

        protected Saga(ICommandBus commandBus, IEventBus eventBus)
        {
            this.commandBus = commandBus;
            this.eventBus = eventBus;
        }

        
    }
}
