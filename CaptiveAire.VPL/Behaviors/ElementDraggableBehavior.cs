using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Behaviors
{
    public class ElementDraggableBehavior : Behavior<FrameworkElement>
    {
        private bool _isMouseDown;
        private Point _startPosition;

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;

            base.OnDetaching();
        }

        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PerformOperation(element =>
            {
                if (!element.IsSelected)
                {
                    element.Owner.SelectionService.SelectWithKeyboardModifiers(element);
                }
            });

            _isMouseDown = true;
            _startPosition = e.GetPosition(null);

            e.Handled = true;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                var position = e.GetPosition(null);

                var distanceMoved = _startPosition - position;

                if (Math.Abs(distanceMoved.X) >= SystemParameters.MinimumHorizontalDragDistance &&
                    Math.Abs(distanceMoved.Y) >= SystemParameters.MinimumVerticalDragDistance)
                {
                    e.Handled = true;

                    PerformOperation(element =>
                    {
                        //Make sure that the element is selected
                        element.Owner.SelectionService.EnsureSelected(element);

                        //Perform the drag operation.
                        element.Owner.SelectionService.DragSelected(AssociatedObject);
                    });
                   

                    _isMouseDown = false;
                }
            }
        }

        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private IElement Element
        {
            get { return AssociatedObject.DataContext as IElement; }
        }

        private FrameworkElement DesignRoot
        {
            get { return AssociatedObject.FindVisualParent<FrameworkElement>("DesignRoot"); }
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