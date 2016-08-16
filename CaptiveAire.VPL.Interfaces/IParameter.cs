using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IParameter : IElement, ITyped, IElementDropTarget
    {
        string Id { get; }

        string Prefix { get; set; }

        string Postfix { get; set; }

        object GetValue();

        void SetValue(object value);

        Task<object> EvaluateAsync(IExecutionContext executionContext, CancellationToken cancellationToken);

        Visual Editor { get; }
    }
}