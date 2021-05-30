using System.Windows.Controls;

namespace Solarponics.SensorModuleEmulator
{
    public partial class ProvisioningView : UserControl
    {
        public ProvisioningView ()
        {
            InitializeComponent();
            DataContext = new ProvisioningViewModel();
        }
    }
}