using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IFunctionService
    {
        IEnumerable<FunctionMetadata> GetFunctions();

        FunctionMetadata GetFunction(Guid id);
    }
}