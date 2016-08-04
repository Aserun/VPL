using System.Threading;
using System.Threading.Tasks;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Represents an executable statement.
    /// </summary>
    public interface IStatement : IElement
    {
        /// <summary>
        /// Executs the statement.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task ExecuteAsync(CancellationToken token);
    }
}