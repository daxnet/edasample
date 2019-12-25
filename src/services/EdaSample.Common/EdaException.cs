using System;
using System.Collections.Generic;
using System.Text;

namespace EdaSample.Common
{
    public class EdaException : Exception
    {
        public EdaException() { }

        public EdaException(string message) : base(message) { }

        public EdaException(string message, Exception innerException) : base(message, innerException) { }
    }
}
