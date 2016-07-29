namespace CaptiveAire.VPL.Metadata
{
    public class ParameterMetadata
    {
        public string Id { get; set; } 

        public object Value { get; set; }

        public ElementMetadata Operator { get; set; }
    }
}