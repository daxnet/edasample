using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdaSample.Common.Repositories
{
    public abstract class Repository : IRepository
    {
        public Task<IAggregateRootWithEventSourcing> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(IAggregateRootWithEventSourcing aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected abstract Task<IAggregateRootWithEventSourcing> RetrieveAggregateByIdAsync(Guid id);

        protected abstract Task PersistAggregate(IAggregateRootWithEventSourcing aggregateRoot);
    }
}
