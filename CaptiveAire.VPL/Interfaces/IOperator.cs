using System.Threading;
using System.Threading.Tasks;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IOperator : IElement, ITyped
    {
        Task<object> EvaluateAsync(CancellationToken token);
    }
}