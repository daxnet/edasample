using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Sagas
{
    public interface ICanHandle<TEvent>
        where TEvent : IEvent
    {
        Task<bool> HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}