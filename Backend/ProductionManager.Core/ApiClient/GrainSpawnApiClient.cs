using Refit;
using Solarponics.Models;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.ApiClient
{
    public class GrainSpawnApiClient : IGrainSpawnApiClient
    {
        private readonly IGrainSpawnApiClient refitClient;

        public GrainSpawnApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<IGrainSpawnApiClient>(settings.ApiBaseUrl, refitSettings);
        }

        public async Task<GrainSpawn> Add(GrainSpawnAddRequest request)
        {
            return await this.refitClient.Add(request);
        }

        public async Task Mix(int id, GrainSpawnAddMixRequest request)
        {
            await this.refitClient.Mix(id, request);
        }

        public async Task<GrainSpawn> Innoculate(int id, [Body] GrainSpawnInnoculateRequest request)
        {
            return await this.refitClient.Innoculate(id, request);
        }

        public async Task<GrainSpawn> ShelfPlace(int id, [Body] GrainSpawnShelfPlaceRequest request)
        {
            return await this.refitClient.ShelfPlace(id, request);
        }

        public async Task<GrainSpawn> Get(int id)
        {
            return await this.refitClient.Get(id);
        }

        public async Task<GrainSpawn[]> GetAll()
        {
            return await this.refitClient.GetAll();
        }
    }
}