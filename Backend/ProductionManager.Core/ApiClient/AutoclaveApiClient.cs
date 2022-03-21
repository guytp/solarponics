using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.ApiClient
{
    public class AutoclaveApiClient : IAutoclaveApiClient
    {
        private readonly IAutoclaveApiClient refitClient;

        public AutoclaveApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<IAutoclaveApiClient>(settings.ApiBaseUrl, refitSettings);
        }
        
        public Task<int> Add(Autoclave autoclave)
        {
            return this.refitClient.Add(autoclave);
        }

        public Task<Autoclave[]> Get()
        {
            return this.refitClient.Get();
        }
        
        public Task Delete(int id)
        {
            return this.refitClient.Delete(id);
        }
    }
}