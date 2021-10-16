using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupSensorModuleModbusTcpViewModel : IViewModel
    {
        bool IsUiEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        SensorModuleModbusTcp[] SensorModules { get; }

        SensorModuleModbusTcp SelectedSensorModule { get; set; }

        byte[] SensorNumbers { get; }

        bool IsSensorSelected { get; }

        ICommand DeleteCommand { get; }

        ICommand PrintLabelCommand { get; }

        ICommand AddCommand { get; }

        bool IsAddEnabled { get; }

        string IpAddress { get; set; }

        string Port { get; set; }
        Room SelectedRoom { get; set; }
        Location SelectedLocation { get; set; }
        Room[] Rooms { get; }
        Location[] Locations { get; }
        string SerialNumber { get; set; }
        string Name { get; set; }

        bool IsTemperatureSensorEnabled { get; set; }
        byte? TemperatureSensorNumber { get; set; }
        string TemperatureSensorCriticalLowBelow { get; set; }
        string TemperatureSensorWarningLowBelow { get; set; }
        string TemperatureSensorWarningHighAbove { get; set; }
        string TemperatureSensorCriticalHighAbove { get; set; }

        bool IsHumiditySensorEnabled { get; set; }
        byte? HumiditySensorNumber { get; set; }
        string HumiditySensorCriticalLowBelow { get; set; }
        string HumiditySensorWarningLowBelow { get; set; }
        string HumiditySensorWarningHighAbove { get; set; }
        string HumiditySensorCriticalHighAbove { get; set; }

        bool IsCarbonDioxideSensorEnabled { get; set; }
        byte? CarbonDioxideSensorNumber { get; set; }
        string CarbonDioxideSensorCriticalLowBelow { get; set; }
        string CarbonDioxideSensorWarningLowBelow { get; set; }
        string CarbonDioxideSensorWarningHighAbove { get; set; }
        string CarbonDioxideSensorCriticalHighAbove { get; set; }
    }
}
