using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions.Factories;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.ViewModels;

namespace Solarponics.ProductionManager.Domain
{
    public class PrinterSettingsViewModelFactory : IPrinterSettingsViewModelFactory
    {
        public IPrinterSettingsViewModel Create(PrinterSettings settings)
        {
            return new PrinterSettingsViewModel(settings);
        }
    }
}