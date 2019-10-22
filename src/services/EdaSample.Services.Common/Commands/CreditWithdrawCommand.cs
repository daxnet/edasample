using EdaSample.Common.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Services.Common.Commands
{
    public sealed class CreditWithdrawCommand : ICommand
    {

        public CreditWithdrawCommand(Guid customerId, float creditsToWithdraw)
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            this.CustomerId = customerId;
            this.CreditsToWithdraw = creditsToWithdraw;
        }

        public Guid Id { get; }

        public DateTime Timestamp { get; }

        public float CreditsToWithdraw { get; } 

        public Guid CustomerId { get; }
    }
}
