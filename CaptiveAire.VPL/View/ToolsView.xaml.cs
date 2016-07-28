using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.ViewModel;

namespace CaptiveAire.VPL.View
{
    /// <summary>
    /// Interaction logic for ToolsView.xaml
    /// </summary>
    public partial class ToolsView : UserControl
    {
        public ToolsView()
        {
            InitializeComponent();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            var container = sender as FrameworkElement;

            //The tool will be a generic type, so why screw around?
            var toolViewModel = container?.DataContext as ToolViewModel<IElementFactory>;

            if (toolViewModel == null)
                return;

            DragDrop.DoDragDrop(container, toolViewModel.Tool, DragDropEffects.Copy);
        }
    }
}
