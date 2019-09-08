using EdaSample.Common.Commands;
using EdaSample.Services.Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Services.Orders.CommandHandlers
{
    public class CreateOrderCommandHandler : BaseCommandHandler<CreateOrderCommand>
    {
        public override Task<bool> HandleAsync(CreateOrderCommand message, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}
