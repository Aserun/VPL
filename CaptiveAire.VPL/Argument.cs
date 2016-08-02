using System;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using GalaSoft.MvvmLight;

namespace CaptiveAire.VPL
{
    public class Argument : ViewModelBase, IArgument
    {
        private readonly IElementOwner _owner;
        private readonly ArgumentMetadata _metadata;

        public Argument(IElementOwner owner, ArgumentMetadata metadata)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            _owner = owner;
            _metadata = metadata;
        }

        public Guid Id
        {
            get { return _metadata.Id; }
        }

        public string Name
        {
            get { return _metadata.Name; }
            set
            {
                _metadata.Name = value;
                RaisePropertyChanged();

                //Get the associated variable
                var variable = _owner.GetVariable(Id);

                //Make sure we actually got it.
                if (variable != null)
                {
                    //Change the name of the the associated variable.
                    variable.Name = value;
                }
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