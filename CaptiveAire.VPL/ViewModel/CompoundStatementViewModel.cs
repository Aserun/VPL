using System;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.ViewModel
{
    public abstract class CompoundStatementViewModel : StatementViewModelBase
    {
        protected CompoundStatementViewModel(IElementOwner owner, Guid elementTypeId)
            : base(owner, elementTypeId)
        {
        }
    }
}