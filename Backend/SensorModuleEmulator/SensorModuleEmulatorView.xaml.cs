using System.Windows.Controls;

namespace Solarponics.SensorModuleEmulator
{
    public partial class SensorModuleEmulatorView : UserControl
    {
        public SensorModuleEmulatorView()
        {
            InitializeComponent();
            DataContext = new SensorModuleEmulatorViewModel();
        }
    }
}