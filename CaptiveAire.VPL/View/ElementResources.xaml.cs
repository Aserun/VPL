using System.Windows;

namespace CaptiveAire.VPL.View
{
    public partial class ElementResources
    {
        public ElementResources()
        {
            InitializeComponent();

        }

        public Style ElementStyle
        {
            get { return this["ElementStyle"] as Style; }
        }
    }
}