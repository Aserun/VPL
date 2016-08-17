using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IExecutionContext : IDisposable
    {
        /// <summary>
        /// Executes a function.
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<object> ExecuteFunctionAsync(Guid functionId, object[] parameters, CancellationToken cancellationToken);
    }
}