using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas.States.Builder
{
    public interface IInitializeBuildStep<TSagaData> : ISagaStateMachineBuildStep<TSagaData>
        where TSagaData : ISagaData
    {
    }

    internal sealed class InitializeBuildStep<TSagaData> : SagaStateMachineBuildStep<TSagaData>, IInitializeBuildStep<TSagaData>
        where TSagaData : ISagaData
    {
        private readonly Func<TSagaData, CancellationToken, Task<bool>> initiateCallback;

        public InitializeBuildStep(ISagaStateMachineBuildStep<TSagaData> buildStep, Func<TSagaData, CancellationToken, Task<bool>> initiateCallback) 
            : base(buildStep)
        {
            this.initiateCallback = initiateCallback;
        }

        protected override SagaStateMachine<TSagaData> BuildThis(SagaStateMachineBuilder<TSagaData> context, SagaStateMachine<TSagaData> machine)
        {
            machine.SetInitialState(this.initiateCallback);
            return machine;
        }
    }
}
