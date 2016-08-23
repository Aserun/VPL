using System.Windows;

namespace CaptiveAire.VPL.Plugins.Functions
{
    /// <summary>
    /// Interaction logic for FunctionSelectionDialogView.xaml
    /// </summary>
    public partial class FunctionSelectionDialogView
    {
        public FunctionSelectionDialogView()
        {
            InitializeComponent();
        }

        private void FunctionSelectionDialogView_OnLoaded(object sender, RoutedEventArgs e)
        {
            FunctionsComboBox.Focus();
        }
    }
}
