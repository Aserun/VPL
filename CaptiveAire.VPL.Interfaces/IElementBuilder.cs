using System.Collections.Generic;
using CaptiveAire.VPL.Metadata;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IElementBuilder
    {
        void AddToOwner(IElementOwner owner, IEnumerable<ElementMetadata> rootElements);

        IElement CreateElement(IElementOwner owner, ElementMetadata elementMetadata);

        void LoadFunction(IFunction function, FunctionMetadata functionMetadata);

        IFunction LoadFunction(FunctionMetadata functionMetadata);
    }
}