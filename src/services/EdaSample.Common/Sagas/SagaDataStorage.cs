using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas
{
    public sealed class SagaDataStorage : IAggregateRoot
    {
        public SagaDataStorage() { }

        public Guid Id { get; set; }

        public Guid SagaId { get; set; }

        public string StateType { get; set; }

        public string StateValue { get; set; }
    }
}
