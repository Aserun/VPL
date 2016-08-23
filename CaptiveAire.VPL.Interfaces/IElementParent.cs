using System;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IElementParent
    {
        void RemoveElement(IElement element);

        bool CanDrop(IElementClipboardData data);

        void Drop(IElement element, IElementClipboardData data);
    }
}