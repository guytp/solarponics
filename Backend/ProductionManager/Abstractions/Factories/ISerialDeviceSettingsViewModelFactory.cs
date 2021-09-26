using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Factories
{
    public interface ISerialDeviceSettingsViewModelFactory
    {
        ISerialDeviceSettingsViewModel Create(SerialDeviceSettings settings, SerialDeviceType serialDeviceType);
    }
}