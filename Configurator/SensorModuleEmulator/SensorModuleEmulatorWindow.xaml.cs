using System.Windows;

namespace Solarponics.SensorModuleEmulator
{
    public partial class SensorModuleEmulatorWindow : Window
    {
        public SensorModuleEmulatorWindow()
        {
            InitializeComponent();
            DataContext = new SensorModuleEmulatorViewModel();
        }
    }
}