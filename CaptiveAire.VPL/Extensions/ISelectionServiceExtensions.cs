using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Metadata;
using CaptiveAire.VPL.Model;
using Xceed.Wpf.DataGrid.Export;

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

        public static void DragSelected(this ISelectionService selectionService, DependencyObject dragSource)
        {
            if (selectionService == null) throw new ArgumentNullException(nameof(selectionService));

            var selected = selectionService.GetSelected();

            //Make sure we have something to work with
            if (selected.Length == 0)
                return;

            var elements = selected.OfType<IElement>()
                .ToArray();

            //Make sure that these are all elements
            if (elements.Length != selected.Length)
                return;

            //Serialize for the clipboard
            var momento = new ElementClipboardData(elements);

            //Perform the operation
            var result =  DragDrop.DoDragDrop(dragSource, momento, DragDropEffects.Copy | DragDropEffects.Move);

            if ((result & DragDropEffects.Move) == DragDropEffects.Move)
            {
                //Remove the originals from their spots
                foreach (var element in elements)
                {
                    //Remove the element from where it came from 
                    element.Parent?.RemoveElement(element);
                }
            }

            //Probably redundant, but we'll do it anyway.
            elements[0].Owner.MarkDirty();
        }
    }
}