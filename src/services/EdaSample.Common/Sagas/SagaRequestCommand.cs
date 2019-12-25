using EdaSample.Common.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas
{
    public abstract class SagaRequestCommand : ICommand
    {
        protected SagaRequestCommand(Guid sagaId) => SagaId = sagaId;

        public Guid Id { get; } = Guid.NewGuid();

        public DateTime Timestamp { get; } = DateTime.UtcNow;

        public Guid SagaId { get; }
    }
}
