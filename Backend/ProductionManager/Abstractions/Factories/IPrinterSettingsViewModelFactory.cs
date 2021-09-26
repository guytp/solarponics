using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Factories
{
    public interface IPrinterSettingsViewModelFactory
    {
        IPrinterSettingsViewModel Create(PrinterSettings settings);
    }
}