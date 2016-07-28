using System.Windows;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using Cas.Common.WPF.Behaviors;

namespace CaptiveAire.VPL.Behaviors
{
    public class SelectableBehavior : DraggableBehaviorBase
    {
        public static readonly DependencyProperty SelectionServiceProperty =
            DependencyProperty.Register("SelectionService", typeof(ISelectionService), typeof(SelectableBehavior));

        public ISelectionService SelectionService
        {
            get { return (ISelectionService)GetValue(SelectionServiceProperty); }
            set { SetValue(SelectionServiceProperty, value); }
        }

        private ISelectable Selectable
        {
            get { return AssociatedObject.DataContext as ISelectable; }
        }

        protected override void Clicked(Point location)
        {
            SelectionService.SelectWithKeyboardModifiers(Selectable);
        }
    }
}