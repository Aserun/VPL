using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    public abstract class OperatorBase : Element, IOperator
    {
        protected OperatorBase(IElementCreationContext context) 
            : base(context)
        {
        }
        
        public abstract IVplType Type { get; }

        protected abstract Task<object> EvaluateCoreAsync(IExecutionContext executionContext,
            CancellationToken cancellationToken);
        
        public Task<object> EvaluateAsync(IExecutionContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                return EvaluateCoreAsync(executionContext, cancellationToken);
            }
            catch (Exception ex)
            {
                //Display the error
                SetError(ex.ToString());

                //blech. I just threw up.
                throw;
            }
        }
    }
}