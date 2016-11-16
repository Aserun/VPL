using System;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IElementClipboardData
    {
        IClipboardDataItem[] Items { get; }

        Guid? SourceFunctionId { get; }
    }
}