using System;
using System.Collections.Generic;
using System.Windows;
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
        /// <param name="modal"></param>
        /// <param name="displayName">Optional display name of the function.</param>
        /// <param name="id">An optional id that uniquely identifies this function</param>
        /// <returns></returns>
        [Obsolete]
        void EditFunction(FunctionMetadata metadata, Action<FunctionMetadata> saveAction, bool modal = true, string displayName = null);

        /// <summary>
        /// Edits a function
        /// </summary>
        /// <param name="id"></param>
        /// <param name="saveAction"></param>
        /// <param name="modal"></param>
        /// <param name="displayName"></param>
        /// <param name="owner"></param>
        void EditFunction(Guid id, Action<FunctionMetadata> saveAction, bool modal = true, string displayName = null, Window owner = null);

        /// <summary>
        /// Gets all of the types from the system and plugins.
        /// </summary>
        IEnumerable<IVplType> Types { get; }

        /// <summary>
        /// Creates an execution context
        /// </summary>
        /// <returns></returns>
        IExecutionContext CreateExecutionContext();
    }
}