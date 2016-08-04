using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.ViewModel
{
    internal class ErrorViewModel
    {
        private readonly IError _error;

        public ErrorViewModel(IError error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            _error = error;
        }

        public ErrorLevel Level
        {
            get { return _error.Level; }
        }

        public string Message
        {
            get { return _error.Message; }
        }
    }
}