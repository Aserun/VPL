namespace CaptiveAire.VPL.Metadata
{
    public abstract class ElementMetadataBase
    {
        public string Data { get; set; }

        public ElementMetadata Next { get; set; }

        public ParameterMetadata[] Parameters { get; set; }

    }
}