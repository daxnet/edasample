using System;
using System.Collections.Generic;
using System.Text;
using EdaSample.Common.Events;
using EdaSample.Common.Events.Domain;

namespace EdaSample.Common
{
    public abstract class AggregateRootWithEventSourcing : IAggregateRootWithEventSourcing
    {
        private readonly Guid id;

        protected AggregateRootWithEventSourcing()
            : this(Guid.NewGuid())
        {

        }

        protected AggregateRootWithEventSourcing(Guid id)
        {

        }

        public IEnumerable<IDomainEvent> UncommittedEvents => throw new NotImplementedException();

        public long Version => throw new NotImplementedException();

        public Guid Id => throw new NotImplementedException();

        void IPurgable.Purge()
        {
            throw new NotImplementedException();
        }

        public void Replay(IEnumerable<IDomainEvent> events)
        {
            throw new NotImplementedException();
        }
    }
}
