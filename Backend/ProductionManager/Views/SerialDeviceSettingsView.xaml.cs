using System.Windows;
using System.Windows.Controls;

namespace Solarponics.ProductionManager.Views
{
    public partial class SerialDeviceSettingsView : UserControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("HeaderProperty", typeof(string), typeof(SerialDeviceSettingsView), new PropertyMetadata(string.Empty));

        public string Header
        {
            get { return (string)this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }

        public SerialDeviceSettingsView()
        {
            InitializeComponent();
        }
    }
}