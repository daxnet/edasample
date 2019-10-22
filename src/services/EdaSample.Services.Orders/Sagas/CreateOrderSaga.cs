using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EdaSample.Common.Commands;
using EdaSample.Common.DataAccess;
using EdaSample.Common.Events;
using EdaSample.Common.Messages;
using EdaSample.Common.Sagas;
using EdaSample.Services.Common.Commands;
using EdaSample.Services.Common.Events;

namespace EdaSample.Services.Orders.Sagas
{
    public class CreateOrderSaga : Saga<CreateOrderSagaState, OrderCreatedEvent>, ICanHandle<CreditWithdrewEvent>
    {
        public CreateOrderSaga(ICommandBus commandBus, IEventBus eventBus, IDataAccessObject sagaStore) 
            : base(commandBus, eventBus, sagaStore)
        {
            
        }

        protected override async Task<bool> StartAsync(OrderCreatedEvent message, CancellationToken cancellationToken = default)
        {
            var state = await this.LoadStateAsync();
            state.OrderId = message.SalesOrderId;
            state.CustomerId = message.CustomerId;
            await SaveStateAsync(state);

            await commandBus.PublishAsync(new CreditWithdrawCommand(message.CustomerId, message.TotalAmount));

            return true;
        }

        public Task<bool> HandleAsync(CreditWithdrewEvent @event, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}
