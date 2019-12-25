using EdaSample.Common.Events;
using EdaSample.Common.Sagas.States;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas
{
    public interface ISaga<TSagaData>
        where TSagaData : ISagaData
    {
        Guid SagaId { get; set; }

        Task StartAsync(TSagaData sagaData);

        SagaStateMachine<TSagaData> StateMachine { get; }
    }
}
