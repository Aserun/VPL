using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IParameter : IElement, ITyped
    {
        string Id { get; }

        string Prefix { get; set; }

        string Postfix { get; set; }

        object GetValue();

        void SetValue(object value);

        Task<object> EvaluateAsync(CancellationToken token);

        Visual Editor { get; }
    }

    //public interface IParameter<TValue> : IParameter
    //{
    //    Task<TValue> EvaluateAsync(CancellationToken token);
    //}
}