using System;
using System.Windows;
using System.Windows.Threading;
using CaptiveAire.VPL.Interfaces;

namespace CaptiveAire.VPL.Behaviors
{
    internal class ActivateableBehavior : ViewModelBehavior<Window, IActivateable>
    {
        private void ActivateableOnActivated(object sender, EventArgs eventArgs)
        {
            //Show the window
            if (AssociatedObject.WindowState == WindowState.Minimized)
            {
                AssociatedObject.WindowState = WindowState.Normal;
            }

            Dispatcher.BeginInvoke((Action)(() => AssociatedObject.Activate()),
             DispatcherPriority.ContextIdle, 
             null);
        }

        protected override void Attach(IActivateable viewModel)
        {
            viewModel.Activated += ActivateableOnActivated;
        }

        protected override void Detach(IActivateable viewModel)
        {
            viewModel.Activated -= ActivateableOnActivated;
        }
    }
}