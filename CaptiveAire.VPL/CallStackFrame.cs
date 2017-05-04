using System;
using System.Linq;
using CaptiveAire.VPL.Interfaces;
using GalaSoft.MvvmLight;

namespace CaptiveAire.VPL
{
    internal class CallStackFrame : ViewModelBase, ICallStackFrame
    {
        private readonly IFunction _function;
        private readonly object[] _arguments;
        private readonly int _index;
        private IStatement _currentStatement;

        internal CallStackFrame(IFunction function, object[] arguments, int index)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));


            _function = function;
            _arguments = arguments ?? new object[]{};
            _index = index;
        }

        public string Name
        {
            get { return _function.Name; }
        }

        public string Prototype
        {
            get
            {
                string args = string.Join(", ", _arguments.Select(a => a?.ToString()));

                return $"{Name}({args})";
            }
        }

        public IStatement CurrentStatement
        {
            get { return _currentStatement; }
            set
            {
                _currentStatement = value; 
                RaisePropertyChanged();
            }
        }

        public int Index
        {
            get { return _index; }
        }

        public override string ToString()
        {
            return $"{Prototype} {CurrentStatement?.Factory?.Name}() {CurrentStatement?.Number}";
        }
    }
}