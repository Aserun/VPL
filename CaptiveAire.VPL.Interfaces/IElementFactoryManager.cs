using System;
using System.Collections.Generic;

namespace CaptiveAire.VPL.Interfaces
{
    /// <summary>
    /// Coordinates all of the elementfactories for a scope.
    /// </summary>
    public interface  IElementFactoryManager
    {
        /// <summary>
        /// Gets the factory for a given element type.
        /// </summary>
        /// <param name="elementTypeId"></param>
        /// <returns></returns>
        IElementFactory GetFactory(Guid elementTypeId);

        /// <summary>
        /// Gets all of the factories.
        /// </summary>
        IEnumerable<IElementFactory>  Factories { get; }
    }
}