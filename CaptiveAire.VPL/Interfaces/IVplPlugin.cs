using System.Collections.Generic;
using System.Windows;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IVplPlugin
    {
        string Name { get; } 

        IEnumerable<ResourceDictionary> Resources { get; }

        IEnumerable<IElementFactory> ElementFactories { get; }

        IEnumerable<IVplType> Types { get; }

        IEnumerable<object> Services { get; }
    }
}