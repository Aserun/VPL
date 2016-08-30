using System.Collections.Generic;
using System.Windows;
using Cas.Common.WPF.Interfaces;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Context in which the VPL is edited / executed.
    /// </summary>
    public interface IVplServiceContext
    {
        /// <summary>
        /// All of the element factories in this context.
        /// </summary>
        IElementFactoryManager ElementFactoryManager { get; }
        
        /// <summary>
        /// The custom resources (from plugins).
        /// </summary>
        IEnumerable<ResourceDictionary> CustomResources { get; }

        /// <summary>
        /// Custom services that can be optionally provided by plugins.
        /// </summary>
        IEnumerable<object> Services { get; }

        /// <summary>
        /// The types available in this context.
        /// </summary>
        IEnumerable<IVplType> Types { get; }

        /// <summary>
        /// Gets the element builder.
        /// </summary>
        IElementBuilder ElementBuilder { get; }
    }
}