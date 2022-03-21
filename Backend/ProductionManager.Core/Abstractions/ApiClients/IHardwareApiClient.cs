using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.Abstractions.ApiClients
{
    public interface IHardwareApiClient
    {
        [Get("/hardware/by-machine-name/{machineName}")]
        Task<HardwareSettings> GetSettings(string machineName);

        [Put("/hardware/by-machine-name/{machineName}/scale")]
        [Headers("Authorization: Bearer")]
        Task SetScale(string machineName, ScaleSettings settings);

        [Delete("/hardware/by-machine-name/{machineName}/scale")]
        [Headers("Authorization: Bearer")]
        Task RemoveScale(string machineName);
        
        [Put("/hardware/by-machine-name/{machineName}/barcode-scanner")]
        [Headers("Authorization: Bearer")]
        Task SetBarcodeScanner(string machineName, BarcodeScannerSettings settings);
        
        [Delete("/hardware/by-machine-name/{machineName}/barcode-scanner")]
        [Headers("Authorization: Bearer")]
        Task RemoveBarcodeScanner(string machineName);

        [Put("/hardware/by-machine-name/{machineName}/label-printer/large")]
        [Headers("Authorization: Bearer")]
        Task SetLabelPrinterLarge(string machineName, LabelPrinterSettings settings);

        [Delete("/hardware/by-machine-name/{machineName}/label-printer/large")]
        [Headers("Authorization: Bearer")]
        Task RemoveLabelPrinterLarge(string machineName);

        [Put("/hardware/by-machine-name/{machineName}/label-printer/small")]
        [Headers("Authorization: Bearer")]
        Task SetLabelPrinterSmall(string machineName, LabelPrinterSettings settings);

        [Delete("/hardware/by-machine-name/{machineName}/label-printer/small")]
        [Headers("Authorization: Bearer")]
        Task RemoveLabelPrinterSmall(string machineName);
    }
}