using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdaSample.Common.Repositories
{
    public interface IRepository
    {
        Task SaveAsync(IAggregateRootWithEventSourcing aggregateRoot);

        Task<IAggregateRootWithEventSourcing> GetByIdAsync(Guid id);
    }
}
