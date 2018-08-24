using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Events.Domain
{
    public sealed class AggregateCreatedEvent : DomainEvent
    {
        public AggregateCreatedEvent(Guid newId)
        {
            this.NewId = newId;
        }

        public Guid NewId { get; set; }
    }
}
