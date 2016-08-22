using System;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using Cas.Common.WPF.Behaviors;

namespace CaptiveAire.VPL.Behaviors
{
    public class ElementDraggableBehavior : DraggableBehaviorBase
    {
        public static readonly DependencyProperty UndoProviderProperty =
            DependencyProperty.Register("UndoProvider", typeof(IUndoProvider), typeof(ElementDraggableBehavior), new FrameworkPropertyMetadata(PropertyChanged));

        private IElementDropTarget _currentOver;

        private static void PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as ElementDraggableBehavior)?.Reset();
        }

        public IUndoProvider UndoProvider
        {
            get { return (IUndoProvider)GetValue(UndoProviderProperty); }
            set { SetValue(UndoProviderProperty, value); }
        }

        private IElement Element
        {
            get { return AssociatedObject.DataContext as IElement; }
        }

        private FrameworkElement DesignRoot
        {
            get { return AssociatedObject.FindVisualParent<FrameworkElement>("DesignRoot"); }
        }

        protected override Point GetPositionFromMouse(MouseEventArgs e)
        {
            return e.GetPosition(DesignRoot);
        }

        private void PerformOperation(Action<IElement> action)
        {
            var element = Element;

            if (element == null)
                return;

            action(element);
        }

        private IElementDropTarget GetDropTargetUnderMouse(Point position)
        {
            var designRoot = DesignRoot;

            if (designRoot == null)
                return null;

            var element = Element;

            if (element == null)
                return null;

            var effectivePosition = position - RelativeStartPosition.ToVector() - new Vector(1, 1);

            var dropTarget = designRoot.GetDropTarget(effectivePosition, element.GetType(), element.GetReturnType());

            if (_currentOver != dropTarget)
            {
                if (_currentOver != null)
                {
                    _currentOver.IsDraggingOver = false;
                }

                if (dropTarget != null && dropTarget.CanDrop(element.GetType(), element.GetReturnType()))
                {
                    dropTarget.IsDraggingOver = true;
                }

                _currentOver = dropTarget;
            }

            return dropTarget;
        }

        protected override void StartDrag(Point position)
        {
            PerformOperation(s =>
            {
                s.StartMove();

                s.IsDragging = true;
            });
        }

        protected override void ContinueDrag(Point position)
        {
            PerformOperation(s =>
            {
                var vector = position - StartPosition;

                GetDropTargetUnderMouse(position);

                s.ContinueMove(vector);
            });
        }

        protected override void FinishDrag(Point position)
        {
            //PerformOperation(s =>
            //{
            //    s.IsDragging = false;

            //    var dropTarget = GetDropTargetUnderMouse(position);

            //    if (dropTarget == null)
            //    {
            //        if (s.GetPrevious() == null)
            //        {
            //            s.CompleteMove(position - StartPosition);
            //        }
            //        else
            //        {
            //            s.DisconnectFromPrevious();

            //            s.CompleteMove(position - RelativeStartPosition);
            //        }
            //    }
            //    else
            //    {
            //        if (dropTarget.CanDrop(s.GetType(), s.GetReturnType()))
            //        {
            //            dropTarget.Drop(s);

            //            //Save the undo state
            //            UndoProvider?.SaveUndoState();
            //        }
            //    }
            //});
        }

        protected override void CancelDrag()
        {
            PerformOperation(s =>
            {
                s.IsDragging = false;

                s.CancelMove();
            });
        }

        protected override void Reset()
        {
            var current = _currentOver;

            if (current != null)
            {
                current.IsDraggingOver = false;
            }

            _currentOver = null;

            base.Reset();
        }
    }
}