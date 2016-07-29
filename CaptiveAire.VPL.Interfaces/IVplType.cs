using System;
using System.Windows.Media;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Represents a VPL type.
    /// </summary>
    public interface IVplType 
    {
        /// <summary>
        /// Gets the unique id for this type.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the name of this type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Creates the visual for editing this type.
        /// </summary>
        /// <returns></returns>
        Visual CreateVisual();

        /// <summary>
        /// Gets the default value for this type.
        /// </summary>
        object DefaultValue { get; }

        /// <summary>
        /// The .NET type
        /// </summary>
        Type NetType { get; }
    }
}