using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions.Factories;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.ViewModels;

namespace Solarponics.ProductionManager.Domain
{
    public class SerialDeviceSettingsViewModelFactory : ISerialDeviceSettingsViewModelFactory
    {
        public ISerialDeviceSettingsViewModel Create(SerialDeviceSettings settings, SerialDeviceType serialDeviceType)
        {
            return new SerialDeviceSettingsViewModel(settings, serialDeviceType);
        }
    }
}