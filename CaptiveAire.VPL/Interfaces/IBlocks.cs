using System.Collections.Generic;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IBlocks : IEnumerable<IBlock>
    {
        void Add(IBlock block);

        bool Remove(IBlock block);
    }
}