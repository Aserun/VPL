using System;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Responsible for creating elements of a given type.
    /// </summary>
    public interface IElementFactory : ITool
    {
        /// <summary>
        /// Gets the ElementType.
        /// </summary>
        Guid ElementTypeId { get; }

        /// <summary>
        /// Creates an element.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IElement Create(IElementCreationContext context);

        /// <summary>
        /// Gets the runtime type information.
        /// </summary>
        Type ElementType { get; }

        /// <summary>
        /// Gets the return type for this elemement (if it has one).
        /// </summary>
        Guid? ReturnType { get; }

        /// <summary>
        /// True to show in the toolbox for direct user creation, false otherwise.
        /// </summary>
        bool ShowInToolbox { get; }
    }
}