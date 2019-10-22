using EdaSample.Common.Commands;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas
{
    public abstract class Saga<TState, TEvent> : ISaga<TState, TEvent>
        where TState : ISagaState, new()
        where TEvent : IEvent
    {
        #region Protected Fields

        protected readonly ICommandBus commandBus;
        protected readonly IEventBus eventBus;
        private Guid stateId;

        #endregion Protected Fields

        #region Private Fields

        private readonly IDataAccessObject sagaStore;

        #endregion Private Fields

        #region Protected Constructors

        protected Saga(ICommandBus commandBus, IEventBus eventBus, IDataAccessObject sagaStore)
        {
            this.commandBus = commandBus;
            this.eventBus = eventBus;
            this.sagaStore = sagaStore;
        }

        #endregion Protected Constructors

        #region Protected Properties

        #endregion Protected Properties

        #region Public Methods

        public async Task<bool> HandleAsync(TEvent message, CancellationToken cancellationToken = default)
        {
            var state = new TState();
            await this.SaveStateAsync(state);
            stateId = state.Id;
            return await this.StartAsync(message, cancellationToken);
        }

        public Task<bool> HandleAsync(IMessage message, CancellationToken cancellationToken = default)
        {
            if (message is TEvent)
            {
                return HandleAsync((TEvent)message, cancellationToken);
            }

            return Task.FromResult(false);
        }

        #endregion Public Methods

        #region Protected Methods

        protected async Task<TState> LoadStateAsync() => await sagaStore.GetByIdAsync<TState>(stateId);

        protected async Task SaveStateAsync(TState state)
        {
            var persistedState = await sagaStore.GetByIdAsync<TState>(state.Id);
            if (persistedState != null)
            {
                await sagaStore.UpdateByIdAsync<TState>(state.Id, state);
            }
            else
            {
                await sagaStore.AddAsync(state);
            }
        }

        protected abstract Task<bool> StartAsync(TEvent message, CancellationToken cancellationToken = default);

        #endregion Protected Methods
    }
}
