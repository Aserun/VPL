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

        //private IElementDropTarget GetDropTargetUnderMouse(Point position)
        //{
        //    var designRoot = DesignRoot;

        //    if (designRoot == null)
        //        return null;

        //    var element = Element;

        //    if (element == null)
        //        return null;

        //    var effectivePosition = position - RelativeStartPosition.ToVector() - new Vector(1, 1);

        //    var dropTarget = designRoot.GetDropTarget(effectivePosition, element.GetType(), element.GetReturnType());

        //    if (_currentOver != dropTarget)
        //    {
        //        if (_currentOver != null)
        //        {
        //            _currentOver.IsDraggingOver = false;
        //        }

        //        if (dropTarget != null && dropTarget.CanDrop(element.GetType(), element.GetReturnType()))
        //        {
        //            dropTarget.IsDraggingOver = true;
        //        }

        //        _currentOver = dropTarget;
        //    }

        //    return dropTarget;
        //}

        //protected override void StartDrag(Point position)
        //{
        //    //PerformOperation(s =>
        //    //{
        //    //    s.StartMove();

        //    //    s.IsDragging = true;
        //    //});
        //}

        //protected override void ContinueDrag(Point position)
        //{
        //    //PerformOperation(s =>
        //    //{
        //    //    var vector = position - StartPosition;

        //    //    GetDropTargetUnderMouse(position);

        //    //    s.ContinueMove(vector);
        //    //});
        //}

        //protected override void FinishDrag(Point position)
        //{
        //    //PerformOperation(s =>
        //    //{
        //    //    s.IsDragging = false;

        //    //    var dropTarget = GetDropTargetUnderMouse(position);

        //    //    if (dropTarget == null)
        //    //    {
        //    //        if (s.GetPrevious() == null)
        //    //        {
        //    //            s.CompleteMove(position - StartPosition);
        //    //        }
        //    //        else
        //    //        {
        //    //            s.DisconnectFromPrevious();

        //    //            s.CompleteMove(position - RelativeStartPosition);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        if (dropTarget.CanDrop(s.GetType(), s.GetReturnType()))
        //    //        {
        //    //            dropTarget.Drop(s);

        //    //            //Save the undo state
        //    //            UndoProvider?.SaveUndoState();
        //    //        }
        //    //    }
        //    //});
        //}

        //protected override void CancelDrag()
        //{
        //    PerformOperation(s =>
        //    {
        //        s.IsDragging = false;

        //        s.CancelMove();
        //    });
        //}

        //protected override void Reset()
        //{
        //    var current = _currentOver;

        //    if (current != null)
        //    {
        //        current.IsDraggingOver = false;
        //    }

        //    _currentOver = null;

        //    base.Reset();
        //}
    }
}