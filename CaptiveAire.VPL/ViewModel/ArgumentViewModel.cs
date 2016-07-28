using CaptiveAire.VPL.Metadata;
using GalaSoft.MvvmLight;
using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.ViewModel
{
    public class ArgumentViewModel : ViewModelBase, IArgument
    {
        private readonly ArgumentMetadata _metadata;

        public ArgumentViewModel(ArgumentMetadata metadata)
        {
            _metadata = metadata;
        }

        public string Name
        {
            get { return _metadata.Name; }
            set
            {
                _metadata.Name = value;
                RaisePropertyChanged();
            }
        }

        public Guid TypeId
        {
            get { return _metadata.TypeId; }
            set
            {
                _metadata.TypeId = value;
                RaisePropertyChanged();
            }
        }
    }
}