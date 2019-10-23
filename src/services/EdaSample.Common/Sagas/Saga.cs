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
        where TState : class, ISagaState, new()
        where TEvent : IEvent
    {

        #region Protected Fields

        protected readonly ICommandBus commandBus;
        protected readonly IEventBus eventBus;

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

        #region Public Properties

        public Guid Id { get; } = Guid.NewGuid();

        #endregion Public Properties

        #region Public Methods

        public async Task<bool> HandleAsync(TEvent message, CancellationToken cancellationToken = default)
        {
            await this.SaveStateAsync();
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

        protected async Task<TState> LoadStateAsync()
        {
            var sagaStorageEntity = (await sagaStore.FindBySpecificationAsync<SagaStorageEntity>(e => e.SagaId == Id)).FirstOrDefault();
            if (sagaStorageEntity == null)
            {
                throw new InvalidOperationException();
            }

            var state = new TState();
            state.Deserialize(sagaStorageEntity.State);
            return state;
        }

        protected async Task SaveStateAsync(TState state = null)
        {
            var persistedSaga = (await sagaStore.FindBySpecificationAsync<SagaStorageEntity>(e => e.SagaId == Id)).FirstOrDefault();
            if (persistedSaga != null)
            {
                persistedSaga.State = state.Serialize();
                await sagaStore.UpdateByIdAsync(persistedSaga.Id, persistedSaga);
            }
            else
            {
                var sagaStorageEntity = new SagaStorageEntity(Id);
                sagaStorageEntity.StateClrType = typeof(TState).AssemblyQualifiedName;
                if (state != null)
                {
                    sagaStorageEntity.State = state.Serialize();
                }

                await sagaStore.AddAsync(sagaStorageEntity);
            }
        }

        protected abstract Task<bool> StartAsync(TEvent message, CancellationToken cancellationToken = default);

        #endregion Protected Methods

    }
}
