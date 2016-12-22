using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

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

            switch (Keyboard.Modifiers)
            {
                case ModifierKeys.Control:

                    if (selectable.IsSelected)
                    {
                        selectionService.Unselect(selectable);
                    }
                    else
                    {
                        selectionService.Select(selectable);
                    }

                    break;

                case ModifierKeys.Shift:

                    selectionService.ShiftSelect(selectable);

                    break;

                case ModifierKeys.None:

                    selectionService.SelectNone();
                    selectionService.Select(selectable);

                    break;
            }

        }

        private static void ShiftSelect(this ISelectionService selectionService, ISelectable selectable)
        {
            var siblings = selectable.GetSiblings();

            //See if there are any non sibinglings that need to be 
            var nonSiblingSelected = selectionService.GetSelected()
                .Where(s => !siblings.Contains(s))
                .ToArray();

            foreach (var toUnselect in nonSiblingSelected)
            {
                selectionService.Unselect(toUnselect);
            }

            //Now, only existing siblings should be selected
            var selected = selectionService.GetSelected();

            //If there are none selected, then select all of the sibings up to this one
            if (!selected.Any())
            {
                foreach (var sibling in siblings)
                {
                    selectionService.Select(sibling);

                    if (sibling == selectable)
                        break;
                }
            }
            else
            {
                //Do a range selection
                selectionService.SelectBetween(siblings, selectable, selected.LastOrDefault());            
            }
        }

        private static void SelectBetween(this ISelectionService selectionService, 
            ISelectable[] siblings, 
            ISelectable start, 
            ISelectable end)
        {
            //Select nothing
            selectionService.SelectNone();

            //Determine the start and stop
            var startIndex = siblings.IndexOf(s => s == start);
            var endIndex = siblings.IndexOf(s => s == end);

            //If for some reason the siblings weren't found, bail
            if (startIndex == null || endIndex == null)
                return;

            //Always start from the "first" sibling
            var effectiveStart = Math.Min(startIndex.Value, endIndex.Value);
            var effectiveEnd = Math.Max(startIndex.Value, endIndex.Value);

            //Mark them all as selected.
            for (int index = effectiveStart; index <= effectiveEnd; index++)
            {
                selectionService.Select(siblings[index]);        
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

            if (!selectable.IsSelected)
            {
                selectionService.Select(selectable);
            }
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
                //Unselect these - they're going away.
                selectionService.SelectNone();

                //Remove the originals from their spots
                foreach (var element in elements)
                {
                    //Remove the element from where it came from 
                    element.Parent?.RemoveElement(element);
                }
            }           
        }

        public static void CopySelected(this ISelectionService selectionService)
        {
            if (selectionService == null) throw new ArgumentNullException(nameof(selectionService));

            //Get the selected elements
            var elements = selectionService.GetSelected()
                .OfType<IElement>();
                
            ClipboardUtility.Copy(elements);            
        }
    }
}