using System.Windows;

namespace CaptiveAire.VPL.View
{
    /// <summary>
    /// Interaction logic for SelectTypeDialogView.xaml
    /// </summary>
    internal partial class SelectTypeDialogView
    {
        public SelectTypeDialogView()
        {
            InitializeComponent();
        }

        private void SelectTypeDialogView_OnLoaded(object sender, RoutedEventArgs e)
        {
            TypesComboBox.Focus();
        }
    }
}
