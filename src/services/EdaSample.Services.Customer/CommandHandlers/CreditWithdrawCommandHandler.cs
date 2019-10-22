using EdaSample.Common.Commands;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.Services.Common.Commands;
using EdaSample.Services.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Services.Customer.CommandHandlers
{
    public class CreditWithdrawCommandHandler : BaseCommandHandler<CreditWithdrawCommand>
    {
        private readonly IDataAccessObject dao;
        private readonly IEventBus eventBus;

        public CreditWithdrawCommandHandler(IDataAccessObject dao, IEventBus eventBus)
        {
            this.dao = dao;
            this.eventBus = eventBus;
        }

        public override async Task<bool> HandleAsync(CreditWithdrawCommand message, CancellationToken cancellationToken = default)
        {
            var c = await this.dao.GetByIdAsync<Model.Customer>(message.CustomerId);
            if (c != null)
            {
                c.Credit -= message.CreditsToWithdraw;
                if(c.Credit <0)
                {
                    // TODO: Send CreditWithdrawFailed event.
                }
                else
                {
                    // TODO: The current implementation of the handler is actually
                    // not idempotent, which means that handling the CreditwithdrawCommand
                    // again will cause the credit amount to be reduced multiple times.
                    // Consider using the SagaId or collaboration ID to make sure that
                    // the handler is idempotent.
                    await this.dao.UpdateByIdAsync(message.CustomerId, c);
                    await this.eventBus.PublishAsync(new CreditWithdrewEvent(message.CustomerId));
                }
            }

            return true;
        }
    }
}

