using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas
{
    public sealed class SagaStorageEntity : IAggregateRoot
    {
        public SagaStorageEntity() { }

        public SagaStorageEntity(Guid sagaId)
        {
            SagaId = sagaId;
        }

        public Guid Id { get; set; }

        public Guid SagaId { get; set; }

        public string StateClrType { get; set; }

        public string State { get; set; }
    }
}
