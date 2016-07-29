using System;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// An argument to a function.
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        string Name { get; } 

        /// <summary>
        /// Gets the type id of the argument.
        /// </summary>
        Guid TypeId { get; }
    }
}