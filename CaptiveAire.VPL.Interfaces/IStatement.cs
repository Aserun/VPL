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
        /// <param name="executionContext"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task ExecuteAsync(IExecutionContext executionContext, CancellationToken token);

        /// <summary>
        /// Gets or sets a value indicating whether this item is enabled or not.
        /// </summary>
        bool IsEnabled { get; set; }
    }
}