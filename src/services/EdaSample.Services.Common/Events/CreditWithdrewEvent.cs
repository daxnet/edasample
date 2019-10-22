using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Services.Common.Events
{
    public class CreditWithdrewEvent : IEvent
    {
        public CreditWithdrewEvent(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public Guid CustomerId { get; }
    }
}
