using System;
using System.Runtime.Serialization;

namespace CaptiveAire.VPL
{
    public class ArgumentMismatchException : Exception
    {
        public ArgumentMismatchException()
        {
        }

        public ArgumentMismatchException(string message) : base(message)
        {
        }

        public ArgumentMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ArgumentMismatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}