using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.ApiClient
{
    public class WasteReasonApiClient : IWasteReasonApiClient
    {
        private readonly IWasteReasonApiClient refitClient;

        public WasteReasonApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<IWasteReasonApiClient>(settings.ApiBaseUrl, refitSettings);
        }
        
        public Task<int> Add(WasteReason wasteReason)
        {
            return this.refitClient.Add(wasteReason);
        }

        public Task<WasteReason[]> Get()
        {
            return this.refitClient.Get();
        }
        
        public Task Delete(int id)
        {
            return this.refitClient.Delete(id);
        }
    }
}