using EdaSample.Common.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Events
{
    public interface IEventPublisher : IMessagePublisher
    {
        Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent;
    }
}