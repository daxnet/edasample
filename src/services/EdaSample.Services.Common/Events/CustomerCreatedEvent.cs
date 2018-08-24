using EdaSample.Common.Events;
using System;

namespace EdaSample.Services.Common.Events
{
    public class CustomerCreatedEvent : IEvent
    {
        public CustomerCreatedEvent(Guid customerId, string customerName, string email)
        {
            this.Id = Guid.NewGuid();
            this.Timestamp = DateTime.UtcNow;
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.Email = email;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public Guid CustomerId { get; }

        public string CustomerName { get; }

        public string Email { get; }
    }
}
