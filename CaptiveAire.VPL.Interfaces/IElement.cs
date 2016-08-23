using System;
using System.Collections.Generic;
using System.Windows;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// A programming element.
    /// </summary>
    public interface IElement : IMoveable, IErrorSource, ISelectable
    {
        /// <summary>
        /// Set to true when the element is being dragged.
        /// </summary>
        bool IsDragging { get; set; }

        /// <summary>
        /// Gets or sets the location of this element.
        /// </summary>
        [Obsolete("We're no longer using location. Everything is in a list now.")]
        Point Location { get; set; }

        /// <summary>
        /// Gets the ElementTypeId
        /// </summary>
        Guid ElementTypeId { get; }

        /// <summary>
        /// Gets the custom serialization data for this element.
        /// </summary>
        /// <returns></returns>
        string GetData();

        /// <summary>
        /// Gets the parameters for this element
        /// </summary>
        IParameters Parameters { get; }

        /// <summary>
        /// Gets the child elements.
        /// </summary>
        IBlocks Blocks { get; }

        /// <summary>
        /// Gets the actions associated with this element.
        /// </summary>
        IEnumerable<IElementAction> Actions { get; }

        /// <summary>
        /// Gets the factory that created this element.
        /// </summary>
        IElementFactory Factory { get; }

        /// <summary>
        /// Gets or sets the parent collection of this element. 
        /// </summary>
        IElementParent Parent { get; set; }

        /// <summary>
        /// Gets the owner of this element.
        /// </summary>
        IElementOwner Owner { get; }
    }
}