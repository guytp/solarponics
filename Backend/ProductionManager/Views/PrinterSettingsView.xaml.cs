using System.Windows;
using System.Windows.Controls;

namespace Solarponics.ProductionManager.Views
{
    public partial class PrinterSettingsView : UserControl
    {
        public string PrinterType
        {
            get { return (string)this.GetValue(PrinterTypeProperty); }
            set { this.SetValue(PrinterTypeProperty, value); }
        }
        public static readonly DependencyProperty PrinterTypeProperty = DependencyProperty.Register("PrinterType", typeof(string), typeof(PrinterSettingsView), new PropertyMetadata("Unknown"));

        public PrinterSettingsView()
        {
            InitializeComponent();
        }
    }
}