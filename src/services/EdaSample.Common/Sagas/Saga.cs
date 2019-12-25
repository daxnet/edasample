using EdaSample.Common.Commands;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.Common.Messages;
using EdaSample.Common.Sagas.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas
{
    public abstract class Saga<TSagaData> : ISaga<TSagaData>
        where TSagaData : ISagaData
    {
        private readonly Lazy<SagaStateMachine<TSagaData>> stateMachine;
        private readonly ICommandPublisher commandPublisher;
        // protected readonly SagaStateMachineBuilder<TSagaData> stateMachineBuilder = new SagaStateMachineBuilder<TSagaData>();

        protected Saga(ICommandPublisher commandPublisher)
        {
            this.commandPublisher = commandPublisher;
            stateMachine = new Lazy<SagaStateMachine<TSagaData>>(BuildStateMachine);
        }

        protected abstract SagaStateMachine<TSagaData> BuildStateMachine();

        public Guid SagaId { get; set; }

        public SagaStateMachine<TSagaData> StateMachine => stateMachine.Value;

        public async Task StartAsync(TSagaData sagaData)
        {
            await StateMachine.StartAsync(sagaData);
        }

        protected async Task SendAsync<TRequestCommand>(TRequestCommand command, CancellationToken cancellationToken = default)
            where TRequestCommand : SagaRequestCommand
        {
            await this.commandPublisher.PublishAsync(command, cancellationToken);
        }
    }
}
