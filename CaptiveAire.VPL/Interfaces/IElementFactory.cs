using System;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IElementFactory : ITool
    {
        Guid ElementTypeId { get; }

        IElement Create(IElementCreationContext context);

        Type ElementType { get; }

        Guid? ReturnType { get; }

        bool ShowInToolbox { get; }
    }
}