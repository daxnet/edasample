using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas.States
{
    //public class SagaStateMachineBuilder<TSagaData>
    //    where TSagaData : ISagaData
    //{
    //    private SagaStateMachine<TSagaData> machine;
    //    private SagaState<TSagaData> currentBuildTimeState;

    //    public SagaStateMachineBuilder<TSagaData> StartWith(Func<TSagaData, CancellationToken, Task<bool>> starter)
    //    {
    //        machine = new SagaStateMachine<TSagaData>(starter);
    //        return this; 
    //    }

    //    public SagaStateMachineBuilder<TSagaData> TransitWhen<TReplyEvent>(SagaState<TSagaData> toState)
    //        where TReplyEvent : SagaReplyEvent
    //    {
    //        machine?.BindState(typeof(TReplyEvent), toState);
    //        currentBuildTimeState = toState;

    //        return this;
    //    }

    //    public SagaStateMachineBuilder<TSagaData> TransitWhen<TReplyEvent>(string stateName,
    //        Func<SagaReplyEvent, TSagaData, CancellationToken, Task<bool>> executor)
    //        where TReplyEvent : SagaReplyEvent
    //        => TransitWhen<TReplyEvent>(new SagaState<TSagaData>(stateName, executor));

    //    public SagaStateMachineBuilder<TSagaData> ThenTransitWhen<TReplyEvent>(SagaState<TSagaData> toState)
    //        where TReplyEvent : SagaReplyEvent
    //    {
    //        if (currentBuildTimeState == null)
    //        {
    //            return TransitWhen<TReplyEvent>(toState);
    //        }

    //        machine.BindState(currentBuildTimeState.Name, typeof(TReplyEvent), toState);
    //        currentBuildTimeState = toState;

    //        return this;
    //    }

    //    public SagaStateMachineBuilder<TSagaData> ThenTransitWhen<TReplyEvent>(string stateName,
    //        Func<SagaReplyEvent, TSagaData, CancellationToken, Task<bool>> executor)
    //        where TReplyEvent : SagaReplyEvent
    //        => ThenTransitWhen<TReplyEvent>(new SagaState<TSagaData>(stateName, executor));

    //    public SagaStateMachine<TSagaData> Build() => machine;
    //}
}
