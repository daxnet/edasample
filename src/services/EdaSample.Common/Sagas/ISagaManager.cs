using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas
{
    public interface ISagaManager<TSagaData>
        where TSagaData : ISagaData, new()
    {
        Task<bool> HandleReplyEventAsync(SagaReplyEvent replyMessage, CancellationToken cancellationToken = default);

        Task InitiateAsync(TSagaData sagaState);
    }
}
