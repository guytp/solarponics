using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupHardwareViewModel : IViewModel
    {
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
        bool IsSaveEnabled { get; }
        ICommand SaveCommand { get; }
        ICommand LabelPrintSmallCommand { get; }
        ICommand LabelPrintLargeCommand { get; }
        bool IsUiEnabled { get; }
        bool IsLabelTestSmallEnabled { get; }
        bool IsLabelTestLargeEnabled { get; }
        ISerialDeviceSettingsViewModel BarcodeScanner { get; }
        IPrinterSettingsViewModel LabelPrinterSmall { get; }
        IPrinterSettingsViewModel LabelPrinterLarge { get; }
        public ISerialDeviceSettingsViewModel Scale { get; }
    }
}