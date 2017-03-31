using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// If the plugin supports function calls, this service must be provided.
    /// </summary>
    public interface IFunctionService
    {
        /// <summary>
        /// Get all of the functions.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IFunctionReference> GetFunctions();

        /// <summary>
        /// Load a function given its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FunctionMetadata GetFunction(Guid id);

        /// <summary>
        /// This is used to support "Go To Definition..."
        /// </summary>
        /// <param name="id"></param>
        void EditFunction(Guid id);
    }
}