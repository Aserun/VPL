using System;
using System.Collections.Generic;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// This is the main entrance point for interacting with the VPL environment.
    /// </summary>
    public interface IVplService
    {
        /// <summary>
        /// Edits a function.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="saveAction"></param>
        /// <returns></returns>
        void EditFunction(FunctionMetadata metadata, Action<FunctionMetadata> saveAction);

        /// <summary>
        /// Creates a function that can be executed.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        IFunction CreateRuntimeFunction(FunctionMetadata metadata);

        /// <summary>
        /// Gets all of the types from the system and plugins.
        /// </summary>
        IEnumerable<IVplType> Types { get; }
    }
}