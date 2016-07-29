using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public abstract class CompoundStatement : StatementBase
    {
        protected CompoundStatement(IElementOwner owner, Guid elementTypeId)
            : base(owner, elementTypeId)
        {
        }
    }
}