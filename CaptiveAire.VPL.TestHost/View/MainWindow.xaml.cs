using System.Windows;
using System.Windows.Input;
using CaptiveAire.VPL.TestHost.ViewModel;

namespace CaptiveAire.VPL.TestHost.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FunctionListBoxItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;

            var functionHeader = frameworkElement?.DataContext as FunctionHeaderViewModel;

            if (functionHeader != null && functionHeader.EditCommand.CanExecute(null))
            {
                functionHeader.EditCommand.Execute(null);
            }
        }
    }
}
