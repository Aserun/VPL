using System.Windows;

namespace CaptiveAire.VPL.View
{
    /// <summary>
    /// Interaction logic for ArgumentDialogView.xaml
    /// </summary>
    internal partial class ArgumentDialogView
    {
        public ArgumentDialogView()
        {
            InitializeComponent();
        }

        private void ArgumentDialogView_OnLoaded(object sender, RoutedEventArgs e)
        {
            NameTextBox.Focus();
        }
    }
}
