using System;

namespace EdaSample.Common.Events
{
    public interface IEvent
    {
        Guid Id { get; }

        DateTime Timestamp { get; }
    }
}