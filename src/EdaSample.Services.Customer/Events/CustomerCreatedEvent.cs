using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.Events
{
    public class CustomerCreatedEvent : IEvent
    {
        public CustomerCreatedEvent(string customerName)
        {
            this.Id = Guid.NewGuid();
            this.Timestamp = DateTime.UtcNow;
            this.CustomerName = customerName;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public string CustomerName { get; }
    }
}
