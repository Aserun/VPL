using System;

namespace CaptiveAire.VPL.Metadata
{
    public abstract class ElementMetadataBase
    {
        public string Data { get; set; }

        [Obsolete("This is available for conversion purposes only.")]
        public ElementMetadata Next { get; set; }

        public ParameterMetadata[] Parameters { get; set; }

    }
}