using System.Windows;

namespace CaptiveAire.VPL.View
{
    /// <summary>
    /// Interaction logic for RenameDialog.xaml
    /// </summary>
    public partial class RenameDialog
    {
        public RenameDialog(string name, string caption)
        {
            InitializeComponent();

            NameTextBox.Text = name;
            NameTextBox.Focus();
            NameTextBox.SelectAll();

            Caption.Text = caption;
        }

        public string EditedName
        {
            get { return NameTextBox.Text; }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EditedName))
            {
                MessageBox.Show("Name is required");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
