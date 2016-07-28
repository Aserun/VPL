using System;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IArgument
    {
        string Name { get; } 

        Guid TypeId { get; }
    }
}