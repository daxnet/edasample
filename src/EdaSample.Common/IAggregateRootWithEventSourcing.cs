using EdaSample.Common.Events;
using EdaSample.Common.Events.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common
{
    public interface IAggregateRootWithEventSourcing : IAggregateRoot, IPurgable, IPersistedVersionSetter
    {
        IEnumerable<IDomainEvent> UncommittedEvents { get; }

        void Replay(IEnumerable<IDomainEvent> events);

        long Version { get; }
    }
}
