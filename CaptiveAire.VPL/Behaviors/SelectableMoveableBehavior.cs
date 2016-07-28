//using System.Collections.Generic;
//using System.Linq;
//using System.Windows;
//using Cas.Common.WPF.Behaviors;
//using VPL.Interfaces;
//using VPL.Model;

//namespace VPL.Behaviors
//{
//    /// <summary>
//    /// Moves the associated object with no relation to ISelectionService.
//    /// </summary>
//    public class SelectableMoveableBehavior : DraggableBehaviorBase
//    {
//        public static readonly DependencyProperty SelectionServiceProperty =
//             DependencyProperty.Register("SelectionService", typeof(ISelectionService), typeof(SelectableMoveableBehavior), new FrameworkPropertyMetadata(PropertyChanged));

//        public static readonly DependencyProperty UndoProviderProperty =
//            DependencyProperty.Register("UndoProvider", typeof(IUndoProvider), typeof(SelectableMoveableBehavior), new FrameworkPropertyMetadata(PropertyChanged));

//        private MoveableDragger _moveableDragger;

//        public ISelectionService SelectionService
//        {
//            get { return (ISelectionService)GetValue(SelectionServiceProperty); }
//            set { SetValue(SelectionServiceProperty, value); }
//        }

//        public IUndoProvider UndoProvider
//        {
//            get { return (IUndoProvider)GetValue(UndoProviderProperty); }
//            set { SetValue(UndoProviderProperty, value); }
//        }

//        private static void PropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
//        {
//            var manager = obj as SelectableMoveableBehavior;

//            manager?.Reset();
//        }

//        private ISelectable Selectable
//        {
//            get { return AssociatedObject.DataContext as ISelectable; }
//        }

//        /// <summary>
//        /// Select this control
//        /// </summary>
//        private void Select()
//        {
//            SelectionService?.SelectWithKeyboardModifiers(Selectable);
//        }

//        protected override void StartDrag(Point position)
//        {
//            SelectionService.EnsureSelected(Selectable);

//            IEnumerable<IMoveable> moveables = SelectionService?.GetSelected()
//                     .OfType<IMoveable>()
//                     .ToArray();

//            _moveableDragger = new MoveableDragger(position, moveables);
//        }
        
//        protected override void ContinueDrag(Point position)
//        {
//            _moveableDragger.Drag(position);
//        }

//        protected override void FinishDrag(Point position)
//        {
//            _moveableDragger?.Complete(position);

//            UndoProvider?.SaveUndoState();

//            _moveableDragger = null;
//        }

//        protected override void CancelDrag()
//        {
//            _moveableDragger?.Cancel();

//            _moveableDragger = null;
//        }

//        protected override void Clicked(Point point)
//        {
//            Select();
//        }

//        protected override void Reset()
//        {
//            //Cancel any pending move operation
//            _moveableDragger?.Cancel();

//            //Ditch the dragger
//            _moveableDragger = null;
               
//            base.Reset();
//        }
//    }
//}