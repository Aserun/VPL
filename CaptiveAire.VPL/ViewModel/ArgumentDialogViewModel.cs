using System;
using System.Collections.Generic;
using System.Windows.Input;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using Cas.Common.WPF.Behaviors;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CaptiveAire.VPL.ViewModel
{
    internal class ArgumentDialogViewModel : ViewModelBase, ICloseableViewModel
    {
        private readonly ArgumentMetadata _argumentMetadata;
        private readonly IEnumerable<IVplType> _types;
        private bool _isDirty;

        public ArgumentDialogViewModel(ArgumentMetadata argumentMetadata, IEnumerable<IVplType> types)
        {
            if (argumentMetadata == null) throw new ArgumentNullException(nameof(argumentMetadata));
            if (types == null) throw new ArgumentNullException(nameof(types));

            _argumentMetadata = argumentMetadata;
            _types = types;

            OkCommand = new RelayCommand(Ok, CanOk);
        }

        public ICommand OkCommand { get; }

        private void Ok()
        {
            Close?.Invoke(this, new CloseEventArgs(true));
        }

        private bool CanOk()
        {
            return _isDirty && ! string.IsNullOrWhiteSpace(Name) && VplTypeId != Guid.Empty;
        }

        public string Name
        {
            get { return _argumentMetadata.Name; }
            set
            {
                _argumentMetadata.Name = value;
                RaisePropertyChanged();
                _isDirty = true;
            }
        }

        public Guid VplTypeId
        {
            get { return _argumentMetadata.TypeId; }
            set
            {
                _argumentMetadata.TypeId = value;
                RaisePropertyChanged();
                _isDirty = true;
            }
        }

        public IEnumerable<IVplType> Types
        {
            get { return _types; }
        }

        public ArgumentMetadata ToMetadata()
        {
            return _argumentMetadata;
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