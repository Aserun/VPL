using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IExecutionContext : IDisposable
    {
        /// <summary>
        /// Executes the root function. Call this to begin execution.
        /// </summary>
        /// <param name="function"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> ExecuteAsync(IFunction function, object[] parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a function. To be used by the internal framework. Not intended to be called by consumers.
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> ExecuteFunctionAsync(Guid functionId, object[] parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Executes a list of statements. To be used by the internal framework. Not intended to be called by consumers.
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteStatementsAsync(IElements elements, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the call stack for this execution context.
        /// </summary>
        ICallStack CallStack { get; }
    }
}