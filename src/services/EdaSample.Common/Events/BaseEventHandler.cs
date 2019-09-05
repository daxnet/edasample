using System.Threading;
using System.Threading.Tasks;
using EdaSample.Common.Messages;

namespace EdaSample.Common.Events
{
    public abstract class BaseEventHandler<TEvent> : MessageHandler<TEvent>, IEventHandler<TEvent>
        where TEvent : IEvent
    {

    }
}