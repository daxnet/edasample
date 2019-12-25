using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas.States.Builder
{
    public static class SagaStateMachineBuildExtensions
    {
        public static ICreateStateMachineBuildStep<TSagaData> CreateStateMachine<TSagaData>(this SagaStateMachineBuilder<TSagaData> stateMachineBuilder)
            where TSagaData : ISagaData
        {
            return new CreateStateMachineBuildStep<TSagaData>(stateMachineBuilder, new SagaStateMachine<TSagaData>());
        }

        public static IInitializeBuildStep<TSagaData> InitializeWith<TSagaData>(this ICreateStateMachineBuildStep<TSagaData> createStateMachineBuildStep,
            Func<TSagaData, CancellationToken, Task<bool>> initiateCallback)
            where TSagaData : ISagaData
        {
            return new InitializeBuildStep<TSagaData>(createStateMachineBuildStep, initiateCallback);
        }

        public static IWhenEventBuildStep<TSagaData> When<TSagaData>(this IInitializeBuildStep<TSagaData> initializeBuildStep, Type replyEventType)
            where TSagaData : ISagaData
        {
            return new WhenEventBuildStep<TSagaData>(initializeBuildStep, replyEventType);
        }

        public static ITransitToBuildStep<TSagaData> TransitTo<TSagaData>(this IWhenEventBuildStep<TSagaData> whenEventBuildStep, SagaState<TSagaData> sagaState)
            where TSagaData : ISagaData
        {
            return new TransitToBuildStep<TSagaData>(whenEventBuildStep, sagaState);
        }

        public static ITransitToBuildStep<TSagaData> TransitTo<TSagaData>(this IWhenEventBuildStep<TSagaData> whenEventBuildStep, string toStateName,
            Func<SagaReplyEvent, TSagaData, CancellationToken, Task<bool>> stateExecutor)
            where TSagaData : ISagaData
        {
            return new TransitToBuildStep<TSagaData>(whenEventBuildStep, new SagaState<TSagaData>(toStateName, stateExecutor));
        }
    }
}
