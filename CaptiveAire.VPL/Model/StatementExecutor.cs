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
        public async Task ExecuteAsync(IStatement statement, CancellationToken token)
        {
            IStatement current = statement;

            while (current != null)
            {
                //Execute the statement
                await current.ExecuteAsync(token);

                //Move to the next statement
                current = current.Next as IStatement;
            }
        }
    }
}