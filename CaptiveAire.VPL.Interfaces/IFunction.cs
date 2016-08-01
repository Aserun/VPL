using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IFunction : IElementOwner
    {
        Guid Id { get; }

        string Name { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        Guid? ReturnTypeId { get; set; }

        Task<object> ExecuteAsync(object[] parameters, CancellationToken cancellationToken);
    }
}