using System.Windows.Input;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Extensions
{
    public static class ISelectionServiceExtensions
    {
        /// <summary>
        /// Selects <paramref name="selectable"/> and considers the Control/Shift buttons.
        /// </summary>
        /// <param name="selectionService"></param>
        /// <param name="selectable"></param>
        public static void SelectWithKeyboardModifiers(this ISelectionService selectionService, ISelectable selectable)
        {
            if (selectionService == null)
                return;

            if (selectable == null)
                return;

            //Determine if we're adding to the selection
            var isAddingToSelection = Keyboard.Modifiers == ModifierKeys.Control ||
                                        Keyboard.Modifiers == ModifierKeys.Shift;

            //See if this item is aready selected
            if (selectable.IsSelected)
            {
                //It is selected. Unselect it.
                selectionService.Unselect(selectable);
            }
            else
            {
                //Unless we're adding to the selection, unselect everything.
                if (!isAddingToSelection)
                {
                    selectionService.SelectNone();
                }

                //Select the current item.
                selectionService.Select(selectable);
            }
        }

        /// <summary>
        /// Ensures that the item is selected (no changes are made if the item is already selected).
        /// </summary>
        /// <param name="selectionService"></param>
        /// <param name="selectable"></param>
        public static void EnsureSelected(this ISelectionService selectionService, ISelectable selectable)
        {
            if (selectionService == null)
                return;

            if (selectable == null)
                return;

            if (selectable.IsSelected)
                return;

            selectionService.SelectWithKeyboardModifiers(selectable);
        }
    }
}