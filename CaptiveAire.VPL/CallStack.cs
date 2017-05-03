using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL
{
    internal class CallStack : ObservableStack<ICallStackFrame>, ICallStack
    {
        public ICallStackFrame CurrentFrame
        {
            get
            {
                if (Count > 0)
                    return Peek();

                return null;
            }
        }
    }
}