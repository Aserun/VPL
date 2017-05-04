using System.Collections.Generic;

namespace CaptiveAire.VPL.Interfaces
{
    public interface ICallStack : IEnumerable<ICallStackFrame>
    {
        ICallStackFrame CurrentFrame { get; }
    }
}