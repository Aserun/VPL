using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    internal class ReturnValueVariable : Variable
    {
        internal static readonly Guid ReturnVariableId = new Guid("F98B9111-0351-4448-8A1C-108CEE324C11");

        public ReturnValueVariable(IElementOwner owner, IVplType type) 
            : base(owner, type, ReturnVariableId)
        {
        }

        protected override bool CanRename()
        {
            return false;
        }

        public override string Name
        {
            get { return "Return"; }
            set {  }
        }

        public override bool Persist
        {
            get { return false; }
        }
    }
}