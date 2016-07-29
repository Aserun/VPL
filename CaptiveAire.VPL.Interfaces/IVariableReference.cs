using System;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// This is used by variable setters and getters. We look for this when attempting to delete a variable from a function.
    /// </summary>
    public interface IVariableReference
    {
        /// <summary>
        /// Gets the id of the referenced variable
        /// </summary>
        Guid VariableId { get; } 
    }
}