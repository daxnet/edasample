using EdaSample.Common.Commands;
using EdaSample.Common.Sagas;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Services.Common.Commands
{
    public sealed class CreditWithdrawCommand : SagaRequestCommand
    {

        public CreditWithdrawCommand(Guid sagaId, Guid customerId, float creditsToWithdraw)
            : base(sagaId)
        {
            this.CustomerId = customerId;
            this.CreditsToWithdraw = creditsToWithdraw;
        }

        public float CreditsToWithdraw { get; }

        public Guid CustomerId { get; }
    }
}
