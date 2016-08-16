using System.Threading;
using System.Threading.Tasks;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Implemented by anything that returns a value.
    /// </summary>
    public interface IOperator : IElement, ITyped
    {
        /// <summary>
        /// Evalutes the operator.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<object> EvaluateAsync(IExecutionContext executionContext, CancellationToken token);
    }
}