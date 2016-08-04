using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public class Error : IError
    {
        private readonly IErrorSource _source;
        private readonly string _message;
        private readonly ErrorLevel _errorLevel;

        public Error(IErrorSource source, string message, ErrorLevel errorLevel)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            _source = source;
            _message = message;
            _errorLevel = errorLevel;
        }

        public IErrorSource Source
        {
            get { return _source; }
        }

        public string Message
        {
            get { return _message; }
        }

        public ErrorLevel Level
        {
            get { return _errorLevel; }
        }
    }
}