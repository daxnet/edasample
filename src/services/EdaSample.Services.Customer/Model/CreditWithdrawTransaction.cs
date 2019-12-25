using EdaSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.Model
{
    public class CreditWithdrawTransaction : IEntity
    {
        public Guid Id { get; set; }

        public Guid SagaId { get; set; }

        public Guid CustomerId { get; set; }

        public float WithdrawAmount { get; set; }

        public bool Closed { get; set; }
    }
}
