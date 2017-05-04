using System;

namespace CaptiveAire.VPL
{
    /// <summary>
    /// An exception that is generated at runtime.
    /// </summary>
    public class VplRuntimeException : Exception
    {
        private readonly string _vplStackTrace;

        internal VplRuntimeException(string message, string vplStackTrace, Exception innerException)
            : base(message, innerException)
        {
            _vplStackTrace = vplStackTrace;
        }

        public override string StackTrace
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_vplStackTrace))
                    return base.StackTrace;

                return _vplStackTrace;
            }
        }

        public override string ToString()
        {
            return
                $"{Message}{Environment.NewLine}{Environment.NewLine}Stack Trace:{Environment.NewLine}{StackTrace}";
        }
    }
}