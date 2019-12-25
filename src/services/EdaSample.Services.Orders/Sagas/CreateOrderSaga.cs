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
using EdaSample.Common.Sagas.States;
using EdaSample.Common.Sagas.States.Builder;
using EdaSample.Services.Common.Commands;
using EdaSample.Services.Common.Events;

namespace EdaSample.Services.Orders.Sagas
{
    public class CreateOrderSaga : Saga<CreateOrderSagaData>
    {
        #region Public Constructors

        public CreateOrderSaga(ICommandPublisher commandPublisher)
            : base(commandPublisher)
        { }
        #endregion Public Constructors

        #region Protected Methods

        protected override SagaStateMachine<CreateOrderSagaData> BuildStateMachine()
        {
            //return new SagaStateMachineBuilder<CreateOrderSagaData>()
            //    .StartWith(OnStart)
            //    .TransitWhen<CreditWithdrewEvent>("creditWithdrew", OnCreditWithdrew)
            //    .ThenTransitWhen<InventoryReservedEvent>("inventoryReserved", OnInventoryReserved)
            //    .Build();

            return new SagaStateMachineBuilder<CreateOrderSagaData>()
                .CreateStateMachine()
                .InitializeWith(OnStart)
                .When(typeof(CreditWithdrewEvent))
                .TransitTo("creditWithdrew", OnCreditWithdrew)
                .Build();
        }

        #endregion Protected Methods

        #region Private Methods

        private async Task<bool> OnCreditWithdrew(SagaReplyEvent replyMessage, CreateOrderSagaData sagaData, CancellationToken cancellationToken = default)
        {
            var evnt = replyMessage.As<CreditWithdrewEvent>();
            if (evnt == null)
            {
                return false;
            }

            sagaData.CreditWithdrewSuccessful = true;

            await SendAsync(new InventoryReservationCommand(SagaId));

            return true;
        }

        private Task<bool> OnInventoryReserved(SagaReplyEvent replyMessage, CreateOrderSagaData sagaData, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        private async Task<bool> OnStart(CreateOrderSagaData initialData, CancellationToken cancellationToken = default)
        {
            await SendAsync(new CreditWithdrawCommand(SagaId, 
                initialData.SalesOrderDetails.CustomerId, 
                initialData.SalesOrderDetails.TotalAmount), cancellationToken);

            return true;
        }

        #endregion Private Methods
    }
}
