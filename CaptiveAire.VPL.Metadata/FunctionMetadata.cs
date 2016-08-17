using System;
using System.Dynamic;

namespace CaptiveAire.VPL.Metadata
{
    public class FunctionMetadata : IFunctionReference
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public ArgumentMetadata[] Arguments { get; set; }

        public Guid? ReturnTypeId { get; set; }

        public ElementMetadata[] Elements { get; set; }  

        public VariableMetadata[] Variables { get; set; }
    }
}