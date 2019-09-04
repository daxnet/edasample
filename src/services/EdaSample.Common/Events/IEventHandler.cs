using EdaSample.Common.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Events
{
    public interface IEventHandler : IMessageHandler { }

    public interface IEventHandler<in TEvent> : IMessageHandler<TEvent>, IEventHandler
        where TEvent : IEvent
    {
    }
}