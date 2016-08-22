using System;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IElementParent
    {
        void RemoveElement(IElement element);

        bool CanDrop(Type elementType, Guid? returnType);

        void Drop(IElement element, IElement droppedElement);
    }
}