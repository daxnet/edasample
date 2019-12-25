using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas
{
    public class SagaNotFoundException : SagaException
    {
        public SagaNotFoundException(Guid sagaId)
            : base(sagaId, $"Saga [{sagaId}] not found")
        { }
    }
}
