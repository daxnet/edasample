using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas.States.Builder
{
    public sealed class SagaStateMachineBuilder<TSagaData>
        where TSagaData : ISagaData
    {

        internal Type CurrentReplyEventType { get; set; }
    }
}
