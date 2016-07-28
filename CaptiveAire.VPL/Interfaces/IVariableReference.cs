using System;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IVariableReference
    {
        /// <summary>
        /// Gets the id of the referenced variable
        /// </summary>
        Guid VariableId { get; } 
    }
}