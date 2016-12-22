using System;
using System.Windows;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Cas.Common.WPF.Behaviors;

namespace CaptiveAire.VPL.Behaviors
{
    public class ElementDraggableBehavior : DraggableBehaviorBase
    {
        protected override void StartDrag(Point position)
        {
            PerformOperation(element =>
            {
                //Make sure that the element is selected
                element.Owner.SelectionService.EnsureSelected(element);

                //Perform the drag operation.
                element.Owner.SelectionService.DragSelected(AssociatedObject);
            });
        }

        protected override void Clicked(Point position)
        {
            PerformOperation(element =>
            {
                //if (!element.IsSelected)
                element.Owner.SelectionService.SelectWithKeyboardModifiers(element);
            });
        }

        private IElement Element
        {
            get { return AssociatedObject.DataContext as IElement; }
        }

        private void PerformOperation(Action<IElement> action)
        {
            var element = Element;

            if (element == null)
                return;

            action(element);
        }
    }
}