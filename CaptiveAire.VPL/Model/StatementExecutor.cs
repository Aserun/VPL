using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Model
{
    /// <summary>
    /// Handle the execution of a statement (and the chained statements after it).
    /// </summary>
    internal class StatementExecutor
    {
        public async Task ExecuteAsync(IExecutionContext executionContext, IElements elements, CancellationToken cancellationToken)
        {
            foreach (var element in elements)
            {
                var statement = element as IStatement;

                if (statement != null)
                {
                    await statement.ExecuteAsync(executionContext, cancellationToken);
                }
            }
        }
    }
}