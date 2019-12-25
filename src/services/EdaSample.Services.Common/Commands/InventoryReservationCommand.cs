using EdaSample.Common.Sagas;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Services.Common.Commands
{
    public class InventoryReservationCommand : SagaRequestCommand
    {
        public InventoryReservationCommand(Guid sagaId) : base(sagaId)
        {
        }
    }
}
