using System;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IElementParent
    {
        void RemoveElement(IElement element);

        bool CanDrop(IElementClipboardData data);

        void Drop(IElement element, IElementClipboardData data);

        IElement[] GetChildren();
    }

    //public interface IStatementParent
    //{
    //    void RemoveStatement(IStatement statement);

    //    bool CanDrop(IElementClipboardData data);

    //    void Drop(IStatement statement, IElementClipboardData data);

    //    IStatement[] GetChildren();
    //}
}