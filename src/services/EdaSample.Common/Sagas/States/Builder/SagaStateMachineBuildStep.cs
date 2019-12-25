using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas.States.Builder
{
    public interface ISagaStateMachineBuildStep<TSagaData>
        where TSagaData : ISagaData
    {
        SagaStateMachineBuilder<TSagaData> BuildContext { get; }

        SagaStateMachine<TSagaData> Build();
    }

    internal abstract class SagaStateMachineBuildStep<TSagaData> : ISagaStateMachineBuildStep<TSagaData>
        where TSagaData : ISagaData
    {
        private readonly ISagaStateMachineBuildStep<TSagaData> buildStep;

        protected SagaStateMachineBuildStep(ISagaStateMachineBuildStep<TSagaData> buildStep)
        {
            this.buildStep = buildStep;
        }

        public SagaStateMachineBuilder<TSagaData> BuildContext => this.buildStep.BuildContext;

        protected abstract SagaStateMachine<TSagaData> BuildThis(SagaStateMachineBuilder<TSagaData> context, SagaStateMachine<TSagaData> machine);

        public SagaStateMachine<TSagaData> Build()
        {
            return BuildThis(this.buildStep.BuildContext, this.buildStep.Build());
        }
    }
}
