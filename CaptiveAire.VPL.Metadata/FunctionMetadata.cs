using System;
using System.Dynamic;

namespace CaptiveAire.VPL.Metadata
{
    public class FunctionMetadata : IFunctionReference
    {
        public int? MetadataVersion { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        [Obsolete("We're no longer using dimensions for the design surface.")]
        public double Width { get; set; }

        [Obsolete("We're no longer using dimensions for the design surface.")]
        public double Height { get; set; }

        public ArgumentMetadata[] Arguments { get; set; }

        public Guid? ReturnTypeId { get; set; }

        public ElementMetadata[] Elements { get; set; }  

        public VariableMetadata[] Variables { get; set; }
    }
}