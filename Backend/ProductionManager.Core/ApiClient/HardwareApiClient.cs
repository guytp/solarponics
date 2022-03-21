using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.ApiClient
{
    public class HardwareApiClient : IHardwareApiClient
    {
        private readonly IHardwareApiClient refitClient;

        public HardwareApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<IHardwareApiClient>(settings.ApiBaseUrl, refitSettings);
        }
        
        public async Task<HardwareSettings> GetSettings(string machineName)
        {
            return await this.refitClient.GetSettings(machineName);
        }

        public async Task SetScale(string machineName, ScaleSettings settings)
        {
            await this.refitClient.SetScale(machineName, settings);
        }

        public async Task RemoveScale(string machineName)
        {
            await this.refitClient.RemoveScale(machineName);
        }

        public async Task SetBarcodeScanner(string machineName, BarcodeScannerSettings settings)
        {
            await this.refitClient.SetBarcodeScanner(machineName, settings);
        }

        public async Task RemoveBarcodeScanner(string machineName)
        {
            await this.refitClient.RemoveBarcodeScanner(machineName);
        }

        public async Task SetLabelPrinterSmall(string machineName, LabelPrinterSettings settings)
        {
            await this.refitClient.SetLabelPrinterSmall(machineName, settings);
        }

        public async Task SetLabelPrinterLarge(string machineName, LabelPrinterSettings settings)
        {
            await this.refitClient.SetLabelPrinterLarge(machineName, settings);
        }

        public async Task RemoveLabelPrinterLarge(string machineName)
        {
            await this.refitClient.RemoveLabelPrinterLarge(machineName);
        }

        public async Task RemoveLabelPrinterSmall(string machineName)
        {
            await this.refitClient.RemoveLabelPrinterSmall(machineName);
        }
    }
}