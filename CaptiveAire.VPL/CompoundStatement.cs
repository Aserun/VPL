using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public abstract class CompoundStatement : Statement
    {
        protected CompoundStatement(IElementCreationContext context)
            : base(context)
        {
        }
    }
}