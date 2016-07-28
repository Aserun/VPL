using System;
using System.Collections.Generic;

namespace CaptiveAire.VPL.Interfaces
{
    public interface  IElementFactoryManager
    {
        IElementFactory GetFactory(Guid elementTypeId);

        IEnumerable<IElementFactory>  Factories { get; }
    }
}