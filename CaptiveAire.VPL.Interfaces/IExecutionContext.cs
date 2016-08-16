using System;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IExecutionContext
    {
        /// <summary>
        /// Gets the metadata for a given function.
        /// </summary>
        /// <param name="functionId"></param>
        /// <returns></returns>
        FunctionMetadata GetFunctionMetadata(Guid functionId);
    }
}