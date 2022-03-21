using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.ApiClient
{
    public class ShelfApiClient : IShelfApiClient
    {
        private readonly IShelfApiClient refitClient;

        public ShelfApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<IShelfApiClient>(settings.ApiBaseUrl, refitSettings);
        }
        
        public Task<int> Add(Shelf shelf)
        {
            return this.refitClient.Add(shelf);
        }

        public Task<Shelf[]> Get()
        {
            return this.refitClient.Get();
        }
        
        public Task Delete(int id)
        {
            return this.refitClient.Delete(id);
        }
    }
}