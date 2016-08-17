using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CaptiveAire.VPL.Metadata;
using Cas.Common.WPF.Behaviors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CaptiveAire.VPL.Plugins.ViewModel
{
    internal class FunctionSelectionDialogViewModel : ViewModelBase, ICloseableViewModel
    {
        private readonly IEnumerable<IFunctionReference> _functions;
        private IFunctionReference _selectedFunction;

        internal FunctionSelectionDialogViewModel(IEnumerable<IFunctionReference> functions, Guid? selectedFunctionId)
        {
            if (functions == null) throw new ArgumentNullException(nameof(functions));

            _functions = functions;

            OkCommand = new RelayCommand(Ok, CanOk);

            if (selectedFunctionId != null)
            {
                SelectedFunction = functions.FirstOrDefault(f => f.Id == selectedFunctionId.Value);
            }
        }

        public ICommand OkCommand { get; private set; }

        private void Ok()
        {
            Close?.Invoke(this, new CloseEventArgs(true));
        }

        private bool CanOk()
        {
            return SelectedFunction != null;
        }

        public IEnumerable<IFunctionReference> Functions
        {
            get { return _functions; }
        }

        public IFunctionReference SelectedFunction
        {
            get { return _selectedFunction; }
            set
            {
                _selectedFunction = value; 
                RaisePropertyChanged();
            }
        }

        public bool CanClose()
        {
            return true;
        }

        public void Closed()
        {
            
        }

        public event EventHandler<CloseEventArgs> Close;
    }
}