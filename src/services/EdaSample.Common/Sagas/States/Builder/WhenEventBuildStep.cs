using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas.States.Builder
{
    public interface IWhenEventBuildStep<TSagaData> : ISagaStateMachineBuildStep<TSagaData>
        where TSagaData : ISagaData
    {
    }

    internal sealed class WhenEventBuildStep<TSagaData> : SagaStateMachineBuildStep<TSagaData>, IWhenEventBuildStep<TSagaData>
        where TSagaData : ISagaData
    {
        private readonly Type replyEventType;

        public WhenEventBuildStep(ISagaStateMachineBuildStep<TSagaData> buildStep, Type replyEventType) : base(buildStep)
        {
            this.replyEventType = replyEventType;
        }

        protected override SagaStateMachine<TSagaData> BuildThis(SagaStateMachineBuilder<TSagaData> context, SagaStateMachine<TSagaData> machine)
        {
            context.CurrentReplyEventType = replyEventType;
            return machine;
        }
    }
}
