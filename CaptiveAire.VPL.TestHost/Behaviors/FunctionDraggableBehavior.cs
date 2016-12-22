using System;
using System.Windows;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Plugins;
using CaptiveAire.VPL.TestHost.ViewModel;
using Cas.Common.WPF.Behaviors;

namespace CaptiveAire.VPL.TestHost.Behaviors
{
    public class FunctionDraggableBehavior : DraggableBehaviorBase
    {
        protected override void StartDrag(Point position)
        {
            //Start dragging 
            PerformOperation(f =>
            {
                var functionCall = SystemPluginFactory.CreateFunctionCallClipboardData(f.Id);

                DragDrop.DoDragDrop(AssociatedObject, functionCall, DragDropEffects.Link);
            });            
        }

        protected override void Clicked(Point position)
        {
        }

        private FunctionHeaderViewModel Function
        {
            get { return AssociatedObject.DataContext as FunctionHeaderViewModel; }
        }

        private void PerformOperation(Action<FunctionHeaderViewModel> action)
        {
            var function = Function;

            if (function == null)
                return;

            action(function);
        }
    }
}