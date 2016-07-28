using System.Windows;
using System.Windows.Controls;

namespace CaptiveAire.VPL.View
{
    /// <summary>
    /// Interaction logic for TextEditorView.xaml
    /// </summary>
    public partial class TextValueView : UserControl
    {
        public TextValueView()
        {
            InitializeComponent();
        }

        private void ValueTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ValueTextBox.SelectAll();
        }
    }
}
