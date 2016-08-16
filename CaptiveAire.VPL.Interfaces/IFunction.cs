using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IFunction : IElementOwner, IErrorSource
    {
        Guid Id { get; }

        string Name { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        Guid? ReturnTypeId { get; set; }

        Task<object> ExecuteAsync(object[] parameters, IExecutionContext executionContext, CancellationToken cancellationToken);
    }
}