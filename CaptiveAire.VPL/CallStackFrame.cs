using System;
using CaptiveAire.VPL.Interfaces;
using GalaSoft.MvvmLight;

namespace CaptiveAire.VPL
{
    internal class CallStackFrame : ViewModelBase, ICallStackFrame
    {
        private readonly IFunction _function;
        private readonly int _index;
        private IStatement _currentStatement;

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
            return $"{Name}() + Line {CurrentStatement?.Number}";
        }
    }
}