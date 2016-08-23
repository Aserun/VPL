using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IClipboardDataItem
    {
        IElementFactory Factory { get; }

        ElementMetadata ElementMetadata { get; }
    }
}