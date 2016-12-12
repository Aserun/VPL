using System;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IFunctionSelector
    {
        IFunctionReference SelectFunction(Guid? selectedFunctionId);
    }
}