using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.ViewModel
{
    public class ElementAction : IElementAction
    {
        private readonly string _name;
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public ElementAction(string name, Action execute, Func<bool> canExecute = null)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Argument is null or empty", nameof(name));
            if (execute == null) throw new ArgumentNullException(nameof(execute));
            
            _name = name;
            _execute = execute;
            _canExecute = canExecute;
        }

        public string Name
        {
            get { return _name; }
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged;
    }
}