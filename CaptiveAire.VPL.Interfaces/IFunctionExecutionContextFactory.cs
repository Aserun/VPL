using System;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// This is used to provide runtime context.
    /// </summary>
    public interface IFunctionExecutionContextFactory
    {
        /// <summary>
        /// This method will be called before the function executes. When execution stops, Dispose will be called on the created instance. 
        /// </summary>
        /// <returns></returns>
        IDisposable Create();
    }
}