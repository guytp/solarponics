using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Abstractions.Hardware
{
    public interface IHardwareProvider
    {
        Task Start();
        void Stop();

        IBarcodeScanner BarcodeScanner { get; }

        IScale Scale { get; }

        ILabelPrinter LabelPrinterSmall { get; }

        ILabelPrinter LabelPrinterLarge { get; }
    }
}