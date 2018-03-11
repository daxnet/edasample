using EdaSample.Common.Events;
using EdaSample.Common.Events.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EdaSample.Common.Repositories
{
    public abstract class Repository : IRepository
    {
        protected Repository()
        {
            
        }

        public async Task<TAggregateRoot> GetByIdAsync<TAggregateRoot>(Guid id)
            where TAggregateRoot : class, IAggregateRootWithEventSourcing
        {
            var domainEvents = await LoadDomainEventsAsync(typeof(TAggregateRoot), id);
            var aggregateRoot = ActivateAggregateRoot<TAggregateRoot>();
            aggregateRoot.Replay(domainEvents);
            return aggregateRoot;
        }

        public Task SaveAsync<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRootWithEventSourcing
        {
            throw new NotImplementedException();
        }  

        private TAggregateRoot ActivateAggregateRoot<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRootWithEventSourcing
        {
            var constructors = from ctor in typeof(TAggregateRoot).GetTypeInfo().GetConstructors()
                               let parameters = ctor.GetParameters()
                               where parameters.Length == 0 ||
                               (parameters.Length == 1 && parameters[0].ParameterType == typeof(Guid))
                               select new { ConstructorInfo = ctor, ParameterCount = parameters.Length };

            if (constructors.Count() > 0)
            {
                TAggregateRoot aggregateRoot;
                var constructorDefinition = constructors.First();
                if (constructorDefinition.ParameterCount == 0)
                {
                    aggregateRoot = (TAggregateRoot)constructorDefinition.ConstructorInfo.Invoke(null);
                }
                else
                {
                    aggregateRoot = (TAggregateRoot)constructorDefinition.ConstructorInfo.Invoke(new object[] { Guid.NewGuid() });
                }

                // 将AggregateRoot下的所有事件清除。事实上，在AggregateRoot的构造函数中，已经产生了AggregateCreatedEvent。
                aggregateRoot.Purge();
                return aggregateRoot;
            }

            return null;
        }

        protected abstract Task PersistDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents);

        protected abstract Task<IEnumerable<IDomainEvent>> LoadDomainEventsAsync(Type aggregateRootType, Guid id);
    }
}
