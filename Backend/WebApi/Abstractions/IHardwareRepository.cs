using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Abstractions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IHardwareRepository
    {
        Task<HardwareSettings> Get(string machineName);
        Task SetBarcodeScanner(string machineName, BarcodeScannerSettings settings, int userId);
        Task RemoveBarcodeScanner(string machineName, int userId);
        Task SetScale(string machineName, ScaleSettings settings, int userId);
        Task RemoveScale(string machineName, int userId);
        Task SetLabelPrinter(string machineName, LabelPrinterSettings settings, int userId, string printerType);
        Task RemoveLabelPrinter(string machineName, int userId, string printerType);
    }
}