using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.ApiClient
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

        public async Task SetLabelPrinter(string machineName, LabelPrinterSettings settings)
        {
            await this.refitClient.SetLabelPrinter(machineName, settings);
        }

        public async Task RemoveLabelPrinter(string machineName)
        {
            await this.refitClient.RemoveLabelPrinter(machineName);
        }
    }
}