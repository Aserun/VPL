using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Extensions
{
    internal static class FunctionExtensions
    {
        /// <summary>
        /// Provide a "line number" for each statement
        /// </summary>
        internal static void NumberStatements(this IFunction function)
        {
            NumberStatements(new HierarchicalNumber(), function.GetRootElements().OfType<IStatement>());
        }

        private static void NumberStatements(HierarchicalNumber startingNumber, IEnumerable<IStatement> statements)
        {
            HierarchicalNumber currentNumber = startingNumber;

            foreach (IStatement statement in statements)
            {
                statement.Number = currentNumber.ToString();

                HierarchicalNumber currentBlockNumber = currentNumber.CreateFirstChild();

                foreach (IBlock block in statement.Blocks)
                {
                    block.Number = currentBlockNumber.ToString();

                    NumberStatements(currentBlockNumber.CreateFirstChild(), block.Elements.OfType<IStatement>());

                    currentBlockNumber = currentBlockNumber.CreateNextSibling();
                }

                currentNumber = currentNumber.CreateNextSibling();
            }
        }

    }
}