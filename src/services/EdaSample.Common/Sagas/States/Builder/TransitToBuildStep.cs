using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas.States.Builder
{
    public interface ITransitToBuildStep<TSagaData> : ISagaStateMachineBuildStep<TSagaData>
        where TSagaData : ISagaData
    { }

    internal sealed class TransitToBuildStep<TSagaData> : SagaStateMachineBuildStep<TSagaData>, ITransitToBuildStep<TSagaData>
        where TSagaData : ISagaData
    {
        private readonly SagaState<TSagaData> toState;

        public TransitToBuildStep(ISagaStateMachineBuildStep<TSagaData> buildStep, SagaState<TSagaData> toState) : base(buildStep)
        {
            this.toState = toState;
        }

        protected override SagaStateMachine<TSagaData> BuildThis(SagaStateMachineBuilder<TSagaData> context, SagaStateMachine<TSagaData> machine)
        {
            machine.BindState(context.CurrentReplyEventType, this.toState);
            return machine;
        }
    }
}
