using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public abstract class Operator : OperatorBase
    {
        private readonly IVplType _type;

        protected Operator(IElementCreationContext context, IVplType type) 
            : base(context)
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