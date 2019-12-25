using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas.States
{
    public class SagaStateBinding<TSagaData>
        where TSagaData : ISagaData
    {

        public SagaStateBinding(string nextState, Type replyMessageType)
        {
            NextState = nextState;
            ReplyMessageType = replyMessageType;
        }

        public string NextState { get; }

        public Type ReplyMessageType { get; }

        public override string ToString()
            => $"when [{ReplyMessageType.Name}] transit to [{NextState}]";
    }
}
