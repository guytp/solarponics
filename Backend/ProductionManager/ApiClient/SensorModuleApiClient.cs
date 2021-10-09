using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.ApiClient
{
    public class SensorModuleApiClient : ISensorModuleApiClient
    {
        private readonly ISensorModuleApiClient refitClient;

        public SensorModuleApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<ISensorModuleApiClient>(settings.ApiBaseUrl, refitSettings);
        }

        public async Task<SensorModule[]> Get()
        {
            return await this.refitClient.Get();
        }
    }
}