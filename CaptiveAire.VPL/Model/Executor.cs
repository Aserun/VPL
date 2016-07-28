using System;
using System.Threading;
using System.Threading.Tasks;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Model
{
    /// <summary>
    /// Handle the execution of a statement (and the chained statements after it).
    /// </summary>
    public class Executor
    {
        public async Task ExecuteAsync(IFunction function, CancellationToken token)
        {
            if (function == null) throw new ArgumentNullException(nameof(function));

            //Find the entrace point of this function.
            var findEntrancePointResult = function.GetEntrancePoint();

            //Check to see if we have an entrance point
            if (findEntrancePointResult.Statement == null)
            {
                throw new EntrancePointNotFoundException(findEntrancePointResult.Error);
            }

            //Execute the statement
            await ExecuteAsync(findEntrancePointResult.Statement, token);
        }

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