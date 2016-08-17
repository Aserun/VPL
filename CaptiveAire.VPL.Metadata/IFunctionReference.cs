using System;

namespace CaptiveAire.VPL.Metadata
{
    public interface IFunctionReference
    {
        Guid Id { get; set; }

        string Name { get; set; }

        ArgumentMetadata[] Arguments { get; set; }

        Guid? ReturnTypeId { get; set; }
    }
}