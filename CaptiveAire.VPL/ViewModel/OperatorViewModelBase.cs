using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.ViewModel
{
    public abstract class OperatorViewModelBase : ElementViewModel, IOperator
    {
        

        protected OperatorViewModelBase(IElementOwner owner, Guid elementTypeId) 
            : base(owner, elementTypeId)
        {
        }

        

        public abstract IVplType Type { get; }

        public abstract Task<object> EvaluateAsync(CancellationToken token);
    }
}