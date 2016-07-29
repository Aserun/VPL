using System;

namespace CaptiveAire.VPL.Interfaces
{
    public interface ISelectionService
    {
        event EventHandler SelectionChanged;

        void SelectNone();

        ISelectable[] GetSelected();

        void Select(ISelectable selectable);

        void Unselect(ISelectable selectable);
    }
}