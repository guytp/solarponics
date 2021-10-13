using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.ApiClient
{
    public class LocationApiClient : ILocationApiClient
    {
        private readonly ILocationApiClient refitClient;

        public LocationApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<ILocationApiClient>(settings.ApiBaseUrl, refitSettings);
        }
        
        public async Task<Location[]> Get()
        {
            return await this.refitClient.Get();
        }

        public async Task<int> Add(Location location)
        {
            return await this.refitClient.Add(location);
        }

        public async Task<int> AddRoom(Room room)
        {
            return await this.refitClient.AddRoom(room);
        }
    }
}