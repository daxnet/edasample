using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Events
{
    public interface IEvent
    {
        Guid Id { get; }

        DateTime Timestamp { get; }
    }
}
