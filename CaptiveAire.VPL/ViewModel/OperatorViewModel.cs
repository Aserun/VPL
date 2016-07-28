using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.ViewModel
{
    public abstract class OperatorViewModel : OperatorViewModelBase
    {
        private readonly IVplType _type;

        protected OperatorViewModel(IElementOwner owner, Guid elementTypeId, IVplType type) 
            : base(owner, elementTypeId)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            _type = type;
        }

        public override IVplType Type
        {
            get { return _type; }
        }
    }
}