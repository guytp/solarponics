using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupHardwareViewModel : IViewModel
    {
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
        bool IsSaveEnabled { get; }
        ICommand SaveCommand { get; }
        ICommand LabelPrintCommand { get; }
        bool IsUiEnabled { get; }
        bool IsLabelTestEnabled { get; }
        ISerialDeviceSettingsViewModel BarcodeScanner { get; }
        IPrinterSettingsViewModel LabelPrinter { get; }
        public ISerialDeviceSettingsViewModel Scale { get; }
    }
}