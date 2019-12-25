using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common.Sagas
{
    public class SagaException : EdaException
    {
        public SagaException(Guid sagaId)
        {
            SagaId = sagaId;
        }

        public SagaException(Guid sagaId, string message) : base(message)
        {
            SagaId = sagaId;
        }

        public SagaException(Guid sagaId, string message, Exception innerException)
            :base(message, innerException)
        {
            SagaId = sagaId;
        }

        public Guid SagaId { get; }
    }
}
