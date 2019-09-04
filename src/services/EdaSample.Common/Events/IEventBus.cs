using EdaSample.Common.Messages;

namespace EdaSample.Common.Events
{
    public interface IEventBus : IMessageBus, IEventPublisher, IEventSubscriber
    {
    }
}