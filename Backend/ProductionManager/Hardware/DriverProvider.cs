using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Exceptions;

namespace Solarponics.ProductionManager.Hardware
{
    public class DriverProvider : IDriverProvider
    {
        private readonly IDialogBox dialogBox;

        public DriverProvider(IDialogBox dialogBox)
        {
            this.dialogBox = dialogBox;
        }

        public THardwareDevice Get<THardwareDevice>(IDriverSettings settings) where THardwareDevice : IHardwareDevice
        {
            if (settings.DriverName == "SerialBarcodeScanner" && typeof(THardwareDevice) == typeof(IBarcodeScanner))
                return (THardwareDevice)(object)new SerialBarcodeScanner((BarcodeScannerSettings)settings);
            
            if (settings.DriverName == "SerialScale" && typeof(THardwareDevice) == typeof(IScale))
                return (THardwareDevice)(object)new SerialScale((ScaleSettings)settings, dialogBox);
            
            if (settings.DriverName == "ZplLabelPrinter" && typeof(THardwareDevice) == typeof(ILabelPrinter))
                return (THardwareDevice)(object)new ZplLabelPrinter((LabelPrinterSettings)settings, dialogBox);

            throw new UnsupportedDriverException(settings.DriverName, typeof(THardwareDevice).FullName);
        }
    }
}