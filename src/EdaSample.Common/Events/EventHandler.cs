using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Events
{
    public abstract class EventHandler<T> : IEventHandler<T>
        where T : IEvent
    {
        public bool CanHandle(IEvent @event)
            => typeof(T).Equals(@event.GetType());

        public abstract Task<bool> HandleAsync(T @event, CancellationToken cancellationToken = default);

        public Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default)
            => CanHandle(@event) ? HandleAsync((T)@event, cancellationToken) : Task.FromResult(false);
    }
}