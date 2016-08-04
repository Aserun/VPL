using System;
using System.Runtime.Serialization;

namespace CaptiveAire.VPL
{
    public class EntrancePointNotFoundException : Exception
    {
        public EntrancePointNotFoundException()
        {
        }

        public EntrancePointNotFoundException(string message) : base(message)
        {
        }

        public EntrancePointNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntrancePointNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}