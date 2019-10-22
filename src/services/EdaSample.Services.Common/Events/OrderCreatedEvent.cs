using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Services.Common.Events
{
    public class OrderCreatedEvent : IEvent
    {
        public OrderCreatedEvent()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public Guid SalesOrderId { get; set; }

        public Guid CustomerId { get; set; }

        public float TotalAmount { get; set; }
    }
}
