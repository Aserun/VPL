using System;
using System.Collections.Generic;
using System.Windows;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IElement : IMoveable, IEntityBase
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsDragging { get; set; }

        /// <summary>
        /// Gets or sets the location of this element.
        /// </summary>
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

        IElement Previous { get; set; }

        IElement Next { get; set; }
    }
}