using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas.States.Builder
{
    public interface ICreateStateMachineBuildStep<TSagaData> : ISagaStateMachineBuildStep<TSagaData>
        where TSagaData : ISagaData
    { }

    internal sealed class CreateStateMachineBuildStep<TSagaData> : ICreateStateMachineBuildStep<TSagaData>
        where TSagaData : ISagaData
    {
        private readonly SagaStateMachine<TSagaData> stateMachine;

        public CreateStateMachineBuildStep(SagaStateMachineBuilder<TSagaData> builder, SagaStateMachine<TSagaData> stateMachine)
        {
            this.BuildContext = builder;
            this.stateMachine = stateMachine;
        }

        public SagaStateMachineBuilder<TSagaData> BuildContext { get; }

        public SagaStateMachine<TSagaData> Build() => this.stateMachine;
    }
}
