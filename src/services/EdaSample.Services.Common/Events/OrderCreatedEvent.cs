using EdaSample.Common.Events;
using EdaSample.Common.Sagas;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Services.Common.Events
{
    public class OrderCreatedEvent : SagaReplyEvent
    {
        public OrderCreatedEvent(Guid sagaId)
            : base(sagaId)
        {
        }

        public Guid SalesOrderId { get; set; }

        public Guid CustomerId { get; set; }

        public float TotalAmount { get; set; }
    }
}
