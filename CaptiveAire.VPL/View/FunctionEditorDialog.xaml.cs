using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Extensions;
using CaptiveAire.VPL.Factory;
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
            this.PerformViewModelAction<FunctionEditorViewModel>(editor =>
            {
                var factory = e.Data.GetData(typeof (ElementFactory)) as IElementFactory;

                if (factory == null)
                    return;

                //Get the position relative to the design surface
                var position = e.GetPosition(DesignRoot);

                //Find the drop target
                var dropTarget = DesignRoot.GetDropTarget(position, factory.ElementType, factory.ReturnType);

                if (drop)
                {
                    if (dropTarget == null)
                    {
                        editor.Function.DropFromToolbox(factory, position);
                    }
                    else
                    {
                        editor.Function.DropFromToolbox(factory, dropTarget);
                    }

                    CurrentDropTarget = null;
                }
                else
                {
                    CurrentDropTarget = dropTarget;
                }

                e.Effects = DragDropEffects.Copy;

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

    }
}
