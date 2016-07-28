using System.Threading;
using System.Threading.Tasks;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IStatement : IElement
    {
        Task ExecuteAsync(CancellationToken token);
    }
}