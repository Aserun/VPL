using System;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IElementDropTarget
    {
        bool IsDraggingOver { get; set; }

        void Drop(IElement element);

        bool CanDrop(Type element, Guid? returnType);
    }
}