using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public class ArgumentVariable : Variable
    {
        private readonly IArgument _argument;

        public ArgumentVariable(IElementOwner owner, IVplType type, IArgument argument) 
            : base(owner, type, argument.Id)
        {
            if (argument == null) throw new ArgumentNullException(nameof(argument));

            _argument = argument;
        }

        protected IArgument Argument
        {
            get { return _argument; }
        }

        protected override bool CanRename()
        {
            return false;
        }
    }
}