using EdaSample.Common.Sagas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.Sagas
{
    public class CreateOrderSagaState : ISagaState
    {
        public CreateOrderSagaState() => Id = Guid.NewGuid();

        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public bool CustomerCreditAccepted { get; set; }

        public bool InventoryAmountAccepted { get; set; }

        public Guid Id { get; }
    }
}
