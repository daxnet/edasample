using EdaSample.Common.Events;
using EdaSample.Common.Sagas;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Services.Common.Events
{
    public class InventoryReservedEvent : SagaReplyEvent
    {
        public InventoryReservedEvent(Guid sagaId)
            : base(sagaId)
        { }
    }
}
