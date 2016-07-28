using System.Windows;
using CaptiveAire.VPL.Interfaces;
using Cas.Common.WPF.Behaviors;

namespace CaptiveAire.VPL.Behaviors
{
    /// <summary>
    /// Moves the associated object with no relation to ISelectionService.
    /// </summary>
    public class SimpleMoveableBehavior : DraggableBehaviorBase
    {
        public static readonly DependencyProperty UndoProviderProperty =
            DependencyProperty.Register("UndoProvider", typeof(IUndoProvider), typeof(SimpleMoveableBehavior), new FrameworkPropertyMetadata(PropertyChanged));

        private static void PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as SimpleMoveableBehavior)?.Reset();
        }

        public IUndoProvider UndoProvider
        {
            get { return (IUndoProvider)GetValue(UndoProviderProperty); }
            set { SetValue(UndoProviderProperty, value); }
        }

        protected override void StartDrag(Point position)
        {
            Moveable?.StartMove();
        }

        protected override void ContinueDrag(Point position)
        {
            var vector = position - StartPosition;

            Moveable?.ContinueMove(vector);
        }

        protected override void FinishDrag(Point position)
        {
            Moveable?.CompleteMove(position - StartPosition);

            //Save the undo state
            UndoProvider?.SaveUndoState();
        }

        protected override void CancelDrag()
        {
            Moveable?.CancelMove();

        }

        private IMoveable Moveable
        {
            get { return AssociatedObject.DataContext as IMoveable; }
        }
    }
}