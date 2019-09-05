using EdaSample.Common.Messages;

namespace EdaSample.Common.Events
{
    public interface IEventBus : IMessageBus<IEvent, IEventHandler>, IEventPublisher, IEventSubscriber
    {
    }
}