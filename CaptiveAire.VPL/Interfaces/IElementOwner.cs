using System.Collections.Generic;
using System.Windows.Controls;

namespace CaptiveAire.VPL.Interfaces
{
    public interface IElementOwner
    {
        void Add(IElement element);

        void Remove(IElement element);

        IEnumerable<IVariable> Variables { get; }

        void AddVariable(IVariable variable);

        void RemoveVariable(IVariable variable);

        IVplServiceContext Context { get; }

        IEnumerable<IElement> GetAllElements();

        IEnumerable<IElement> GetRootElements();

        void AddArgument(IArgument argument);

        void MarkDirty();

        void MarkClean();

        bool IsDirty { get; }

    }
}