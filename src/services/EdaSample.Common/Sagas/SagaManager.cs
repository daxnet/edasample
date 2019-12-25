using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using Newtonsoft.Json;

namespace EdaSample.Common.Sagas
{
    public class SagaManager<TSagaData> : ISagaManager<TSagaData>
        where TSagaData : ISagaData, new()
    {

        #region Private Fields

        private readonly IEventSubscriber eventSubscriber;
        private readonly IDataAccessObject sagaStore;
        private readonly ISaga<TSagaData> saga;

        #endregion Private Fields

        #region Public Constructors

        public SagaManager(ISaga<TSagaData> saga, IEventSubscriber eventSubscriber, IDataAccessObject sagaStore)
        {
            this.saga = saga;
            this.eventSubscriber = eventSubscriber;
            this.sagaStore = sagaStore;
            SubscribeReplyEvents();
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task InitiateAsync(TSagaData sagaData)
        {
            var sagaId = Guid.NewGuid();
            await SaveStateAsync(sagaId, sagaData);

            saga.SagaId = sagaId;
            await saga.StartAsync(sagaData);
        }

        public async Task<bool> HandleReplyEventAsync(SagaReplyEvent replyMessage, CancellationToken cancellationToken = default)
        {
            // Here, we should transit the saga state (state machine) based on the received event.
            // Determine the next step of the saga workflow and transits the state to the next step.
            var sagaId = replyMessage.SagaId;
            TSagaData sagaData;
            try
            {
                sagaData = await LoadStateAsync(sagaId);
            }
            catch (SagaNotFoundException)
            {
                return false;
            }
            catch
            {
                throw;
            }

            var handled = await saga.StateMachine.RaiseEventAsync(replyMessage, sagaData, cancellationToken);
            await SaveStateAsync(sagaId, sagaData);

            return handled;
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<TSagaData> LoadStateAsync(Guid sagaId)
        {
            var sagaStorageEntity = (await sagaStore.FindBySpecificationAsync<SagaDataStorage>(e => e.SagaId == sagaId)).FirstOrDefault();
            if (sagaStorageEntity == null)
            {
                throw new SagaNotFoundException(sagaId);
            }

            var state = JsonConvert.DeserializeObject<TSagaData>(sagaStorageEntity.StateValue);
            return state;
        }

        private async Task SaveStateAsync(Guid sagaId, TSagaData state)
        {
            var persistedSaga = (await sagaStore.FindBySpecificationAsync<SagaDataStorage>(e => e.SagaId == sagaId)).FirstOrDefault();
            if (persistedSaga != null)
            {
                persistedSaga.StateValue = JsonConvert.SerializeObject(state);
                await sagaStore.UpdateByIdAsync(persistedSaga.Id, persistedSaga);
            }
            else
            {
                var storageEntity = new SagaDataStorage
                {
                    SagaId = sagaId,
                    StateType = typeof(TSagaData).AssemblyQualifiedName
                };

                if (state != null)
                {
                    storageEntity.StateValue = JsonConvert.SerializeObject(state);
                }

                await sagaStore.AddAsync(storageEntity);
            }
        }

        private void SubscribeReplyEvents()
        {
            var replyEventTypes = saga.StateMachine.TriggeringEventTypes;
            foreach (var messageType in replyEventTypes)
            {
                this.eventSubscriber.Subscribe(messageType, typeof(SagaManagerInlineEventHandler<TSagaData>));
            }
        }

        #endregion Private Methods
    }


}
