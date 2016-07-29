using System.Collections.Generic;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// A block to be used in a compound statement.
    /// </summary>
    public interface IBlocks : IEnumerable<IBlock>
    {
        /// <summary>
        /// Adds a block
        /// </summary>
        /// <param name="block"></param>
        void Add(IBlock block);

        /// <summary>
        /// Removes a block.
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        bool Remove(IBlock block);
    }
}