using System;

namespace CaptiveAire.VPL.Editors
{
    /// <summary>
    /// Interaction logic for BooleanValueView.xaml
    /// </summary>
    internal partial class BooleanValueRadioButtonView
    {
        public BooleanValueRadioButtonView()
        {
            InitializeComponent();

            var groupName = Guid.NewGuid().ToString();

            OnRadioButton.GroupName = groupName;
            OffRadioButton.GroupName = groupName;
        }
    }
}
