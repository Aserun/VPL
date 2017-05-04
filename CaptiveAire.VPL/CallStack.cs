using System.Linq;
using System.Text;
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

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (ICallStackFrame frame in this)
            {
                result.AppendLine(frame.ToString());
            }

            return result.ToString();
        }
    }
}