using System.Windows;
using System.Windows.Interactivity;

namespace CaptiveAire.VPL.Behaviors
{
    public abstract class ViewModelBehavior<TElement, TViewModel> : Behavior<TElement>
        where TElement : FrameworkElement
        where TViewModel : class
    {
        private TViewModel _viewModel;

        protected sealed override void OnAttached()
        {
            AssociatedObject.DataContextChanged += AssociatedObject_DataContextChanged;

            _viewModel = AssociatedObject.DataContext as TViewModel;

            if (_viewModel != null)
                Attach(_viewModel);

            base.OnAttached();
        }

        protected TViewModel ViewModel
        {
            get {  return _viewModel; }
        }

        protected abstract void Attach(TViewModel viewModel);

        protected abstract void Detach(TViewModel viewModel);

        private void AssociatedObject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_viewModel != null)
                Detach(_viewModel);

            _viewModel = e.NewValue as TViewModel;

            if (_viewModel != null)
                Attach(_viewModel);
        }

        protected sealed override void OnDetaching()
        {
            base.OnDetaching();

            if (_viewModel != null)
                Detach(_viewModel);
        }
    }
}