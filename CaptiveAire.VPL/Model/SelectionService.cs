using System;
using System.Collections.Generic;
using System.Linq;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Model
{
    public class SelectionService : ISelectionService
    {
        public event EventHandler SelectionChanged;

        private readonly List<ISelectable> _selected = new List<ISelectable>();

        public void SelectNone()
        {
            if (_selected.Any())
            {
                _selected.ForEach(s => s.IsSelected = false);

                _selected.Clear();

                OnSelectionChanged();
            }
        }

        protected void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        public ISelectable[] GetSelected()
        {
            return _selected.ToArray();
        }

        public void Select(ISelectable selectable)
        {
            if (selectable == null)
                throw new ArgumentNullException(nameof(selectable));

            selectable.IsSelected = true;

            if (!_selected.Contains(selectable))
            {
                _selected.Add(selectable);

                OnSelectionChanged();
            }
        }

        public void Unselect(ISelectable selectable)
        {
            if (selectable == null)
                throw new ArgumentNullException(nameof(selectable));

            if (_selected.Contains(selectable))
            {
                //Unselect the item
                selectable.IsSelected = false;

                //Remove it from the list of selections
                _selected.Remove(selectable);

                //Raise the event
                OnSelectionChanged();
            }
        }
    }
}