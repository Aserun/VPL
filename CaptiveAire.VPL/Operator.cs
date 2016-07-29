using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public abstract class Operator : OperatorBase
    {
        private readonly IVplType _type;

        protected Operator(IElementOwner owner, Guid elementTypeId, IVplType type) 
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