using System.Threading;
using System.Threading.Tasks;
using EdaSample.Common.Messages;

namespace EdaSample.Common.Events
{
    public abstract class BaseEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public bool CanHandle(IMessage message) => typeof(TEvent).Equals(message.GetType());

        public abstract Task<bool> HandleAsync(TEvent @event, CancellationToken cancellationToken = default);

        public Task<bool> HandleAsync(IMessage message, CancellationToken cancellationToken = default)
            => CanHandle(message) ? HandleAsync((TEvent)message, cancellationToken) : Task.FromResult(false);
    }
}