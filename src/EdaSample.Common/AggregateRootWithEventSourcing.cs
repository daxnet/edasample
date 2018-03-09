using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EdaSample.Common.Events;
using EdaSample.Common.Events.Domain;
using System.Threading;

namespace EdaSample.Common
{
    public abstract class AggregateRootWithEventSourcing : IAggregateRootWithEventSourcing
    {
        private readonly Guid id;
        private readonly ConcurrentQueue<IDomainEvent> uncommittedEvents = new ConcurrentQueue<IDomainEvent>();
        private readonly Lazy<Dictionary<string, MethodInfo>> registeredHandlers;
        private long persistedVersion = 0;

        protected AggregateRootWithEventSourcing()
            : this(Guid.NewGuid())
        {

        }

        protected AggregateRootWithEventSourcing(Guid id)
        {
            Raise(new AggregateCreatedEvent(id));

            registeredHandlers = new Lazy<Dictionary<string, MethodInfo>>(() =>
            {
                var registry = new Dictionary<string, MethodInfo>();
                var methodInfoList = from mi in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                     let returnType = mi.ReturnType
                                     let parameters = mi.GetParameters()
                                     where returnType == typeof(void) &&
                                     parameters.Length == 1 &&
                                     typeof(IDomainEvent).IsAssignableFrom(parameters[0].ParameterType)
                                     select new { EventName = parameters[0].ParameterType.FullName, MethodInfo = mi };

                foreach(var methodInfo in methodInfoList)
                {
                    registry.Add(methodInfo.EventName, methodInfo.MethodInfo);
                }

                return registry;
            });
        }

        public IEnumerable<IDomainEvent> UncommittedEvents => uncommittedEvents;

        public long Version => this.uncommittedEvents.Count + this.persistedVersion;

        public Guid Id => id;

        protected void Raise<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : IDomainEvent
        {
            domainEvent.AggregateRootId = this.id;
            domainEvent.AggregateRootType = this.GetType().AssemblyQualifiedName;
            domainEvent.Sequence = this.Version + 1;

            this.HandleEvent(domainEvent);

            this.uncommittedEvents.Enqueue(domainEvent);
        }

        private void HandleEvent<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : IDomainEvent
        {
            var key = domainEvent.GetType().FullName;
            if (registeredHandlers.Value.ContainsKey(key))
            {
                registeredHandlers.Value[key].Invoke(this, new object[] { domainEvent });
            }
        }

        void IPurgable.Purge()
        {
            while (!uncommittedEvents.IsEmpty)
            {
                uncommittedEvents.TryDequeue(out var _);
            }
        }

        public void Replay(IEnumerable<IDomainEvent> events)
        {
            ((IPurgable)this).Purge();
            events.OrderBy(e => e.Timestamp)
                .ToList()
                .ForEach(e =>
                {
                    HandleEvent(e);
                    Interlocked.Increment(ref this.persistedVersion);
                });
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as AggregateRootWithEventSourcing;
            if (other == null)
            {
                return false;
            }

            return this.id.Equals(other.id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => this.id.GetHashCode();

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(AggregateRootWithEventSourcing a, AggregateRootWithEventSourcing b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(AggregateRootWithEventSourcing a, AggregateRootWithEventSourcing b) => !(a == b);
    }
}
