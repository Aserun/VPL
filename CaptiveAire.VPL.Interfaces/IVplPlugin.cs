using System.Collections.Generic;
using System.Windows;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// A plugin.
    /// </summary>
    public interface IVplPlugin
    {
        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        string Name { get; } 

        /// <summary>
        /// Gets the custom resources for this plugin.
        /// </summary>
        IEnumerable<ResourceDictionary> Resources { get; }

        /// <summary>
        /// Gets the element factories for this plugin.
        /// </summary>
        IEnumerable<IElementFactory> ElementFactories { get; }

        /// <summary>
        /// Gets the custom types for this plugin.
        /// </summary>
        IEnumerable<IVplType> Types { get; }

        /// <summary>
        /// Gets the custom services for this plugin. One use for a custom service is for 
        /// accessing global information from an instance of a VplType editor.
        /// </summary>
        IEnumerable<object> Services { get; }

        /// <summary>
        /// Gets the runtime service factories
        /// </summary>
        /// <returns></returns>
        IEnumerable<IRuntimeServiceFactory> RuntimeServiceFactories { get; }
    }
}