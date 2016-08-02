using System;
using System.Collections.Generic;
using System.Windows.Input;
using CaptiveAire.VPL.Interfaces;
using Cas.Common.WPF.Behaviors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CaptiveAire.VPL.ViewModel
{
    internal class SelectTypeDialogViewModel : ViewModelBase, ICloseableViewModel
    {
        private readonly IEnumerable<IVplType> _types;
        private Guid? _selectedTypeId;

        public SelectTypeDialogViewModel(IEnumerable<IVplType> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            _types = types;
            
            OkCommand = new RelayCommand(Ok, CanOk);
        }

        public ICommand OkCommand { get; }

        public Guid? SelectedTypeId
        {
            get { return _selectedTypeId; }
            set
            {
                _selectedTypeId = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<IVplType> Types
        {
            get { return _types; }
        }

        private void Ok()
        {
            Close?.Invoke(this, new CloseEventArgs(true));
        }

        private bool CanOk()
        {
            return SelectedTypeId != null;
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