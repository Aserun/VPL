using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;
using CaptiveAire.VPL.ViewModel;
using Cas.Common.WPF;

namespace CaptiveAire.VPL.View
{
    /// <summary>
    /// Interaction logic for FunctionEditorDialog.xaml
    /// </summary>
    internal partial class FunctionEditorDialog
    {
        private IElementDropTarget _currentDropTarget;

        public FunctionEditorDialog(IEnumerable<ResourceDictionary> customResources)
        {
            InitializeComponent();

            if (customResources != null)
            {
                foreach (var resourceDictionary in customResources)
                {
                    MainResourceDictionary.MergedDictionaries.Add(resourceDictionary);
                }
            }
        }

        public IElementDropTarget CurrentDropTarget
        {
            get { return _currentDropTarget; }
            set
            {
                if (_currentDropTarget != value)
                {
                    if (_currentDropTarget != null)
                    {
                        _currentDropTarget.IsDraggingOver = false;
                    }

                    _currentDropTarget = value;

                    if (_currentDropTarget != null)
                    {
                        _currentDropTarget.IsDraggingOver = true;
                    }
                }
            }
        }

        private void HandleDrag(DragEventArgs e, bool drop)
        {
            this.PerformViewModelAction<FunctionEditorDialogViewModel>(editor =>
            {
                var data = e.Data.GetData(typeof(ElementClipboardData)) as IElementClipboardData;

                if (data == null)
                {

                    var factory = e.Data.GetData(typeof(ElementFactory)) as IElementFactory;

                    if (factory == null)
                    {
                        e.Effects = DragDropEffects.None;
                        return;
                    }

                    //Create data from the factory
                    data = new ElementClipboardData(factory);
                }

                //Get the position relative to the design surface
                var position = e.GetPosition(DesignRoot);

                //Find the drop target
                var dropTarget = DesignRoot.GetDropTarget(position, data);

                //Check to see if we can do this
                if (dropTarget == null)
                {
                    if (!editor.Function.CanDropFromToolbox(data))
                    {
                        e.Effects = DragDropEffects.None;
                        return;
                    }
                }
                else
                {
                    if (!dropTarget.CanDrop(data))
                    {
                        e.Effects = DragDropEffects.None;
                        return;
                    }
                }

                if ((e.AllowedEffects & DragDropEffects.Move) == DragDropEffects.Move)
                {
                    e.Effects = DragDropEffects.Move;
                }
                else if ((e.AllowedEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
                {
                    e.Effects = DragDropEffects.Copy;
                }

                if (drop)
                {
                    if (dropTarget == null)
                    {
                        editor.Function.DropFromToolbox(data);
                    }
                    else
                    {
                        dropTarget.Drop(data);
                    }

                    CurrentDropTarget = null;
                }
                else
                {
                    CurrentDropTarget = dropTarget;
                }

                e.Handled = true;
            });
        }

        private void DesignRoot_OnDrop(object sender, DragEventArgs e)
        {
            HandleDrag(e, true);
        }

        private void DesignRoot_OnDragOver(object sender, DragEventArgs e)
        {
            HandleDrag(e, false);
        }

        private void DesignRoot_OnDragEnter(object sender, DragEventArgs e)
        {
            HandleDrag(e, false);
        }

        private void DesignRoot_OnDragLeave(object sender, DragEventArgs e)
        {
            CurrentDropTarget = null;
        }

        private void ScaleOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                this.PerformViewModelAction<FunctionEditorDialogViewModel>(vm =>
                {
                    double scaledDelta = e.Delta / 1200.0;

                    double newScale = vm.Scale + scaledDelta;

                    if (newScale < vm.ScaleMin)
                    {
                        newScale = vm.ScaleMin;
                    }

                    if (newScale > vm.ScaleMax)
                    {
                        newScale = vm.ScaleMax;
                    }

                    vm.Scale = newScale;
                });
            }
        }
    }
}
