using System;
using System.Threading.Tasks;

namespace EdaSample.Common.Events
{
    public interface IEventStore : IDisposable
    {
        Task SaveEventAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}