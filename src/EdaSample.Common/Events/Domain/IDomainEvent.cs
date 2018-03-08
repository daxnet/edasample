using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Events.Domain
{
    public interface IDomainEvent : IEvent
    {
        long Sequence { get; set; }

        string AggregateRootType { get; set; }

        Guid AggregateRootId { get; set; }
    }
}
