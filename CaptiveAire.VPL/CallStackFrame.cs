using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    internal class CallStackFrame : ICallStackFrame
    {
        private readonly IFunction _function;
        private readonly int _index;

        internal CallStackFrame(IFunction function, int index)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));


            _function = function;
            _index = index;
        }

        public string Name
        {
            get { return _function.Name; }
        }

        public int Index
        {
            get { return _index; }
        }
    }
}