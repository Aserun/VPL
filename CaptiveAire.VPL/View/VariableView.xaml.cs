using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.Factory;
using CaptiveAire.VPL.Interfaces;
using CaptiveAire.VPL.Model;

namespace CaptiveAire.VPL.View
{
    /// <summary>
    /// Interaction logic for VariableView.xaml
    /// </summary>
    internal partial class VariableView
    {
        public VariableView()
        {
            InitializeComponent();
        }

        private void VariableGetterGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;

            var variable = frameworkElement?.DataContext as IVariable;

            if (variable == null)
                return;

            var elementFactory = new ElementFactory(SystemElementIds.VariableGetter, FactoryCategoryNames.Variable, "Get", context => new VariableGetter(context, variable.Id), typeof(VariableGetter), variable.Type.Id);

            DragDrop.DoDragDrop(frameworkElement, elementFactory, DragDropEffects.Copy);
        }

        private void VariableSetterGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;

            var variable = frameworkElement?.DataContext as IVariable;

            if (variable == null)
                return;

            var elementFactory = new ElementFactory(SystemElementIds.VariableSetter, FactoryCategoryNames.Variable, "Set", context => new VariableSetter(context, variable.Id), typeof(VariableSetter), variable.Type.Id);

            DragDrop.DoDragDrop(frameworkElement, elementFactory, DragDropEffects.Copy);
        }
    }
}

