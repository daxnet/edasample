using EdaSample.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas
{
    public abstract class SagaReplyEvent : IEvent
    {
        protected SagaReplyEvent(Guid sagaId) => SagaId = sagaId;

        public Guid Id { get; } = Guid.NewGuid();

        public DateTime Timestamp { get; } = DateTime.UtcNow;

        public Guid SagaId { get; }

        public TReplyEvent As<TReplyEvent>()
            where TReplyEvent : SagaReplyEvent
        {
            if (this is TReplyEvent re)
            {
                return re;
            }

            return null;
        }
    }
}
