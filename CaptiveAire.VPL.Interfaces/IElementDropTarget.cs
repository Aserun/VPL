using System;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Handles receiving a dropped element.
    /// </summary>
    public interface IElementDropTarget
    {
        /// <summary>
        /// This is available so we know when the element is being dragged over and it can optionally highlight itself.
        /// </summary>
        bool IsDraggingOver { get; set; }

        ///// <summary>
        ///// Drops the element.
        ///// </summary>
        ///// <param name="element"></param>
        //void Drop(IElement element);

        ///// <summary>
        ///// Determines whether the item can be dropped.
        ///// </summary>
        ///// <param name="element"></param>
        ///// <param name="returnType"></param>
        ///// <returns></returns>
        //bool CanDrop(Type element, Guid? returnType);

        bool CanDrop(IElementClipboardData data);

        void Drop(IElementClipboardData data);
    }
}