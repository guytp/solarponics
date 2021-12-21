using Refit;
using Solarponics.Models;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.ApiClient
{
    public class FruitingBlockApiClient : IFruitingBlockApiClient
    {
        private readonly IFruitingBlockApiClient refitClient;

        public FruitingBlockApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<IFruitingBlockApiClient>(settings.ApiBaseUrl, refitSettings);
        }
        
        public async Task<FruitingBlock> Add(FruitingBlockAddRequest request)
        {
            return await this.refitClient.Add(request);
        }

        public async Task<FruitingBlock> Innoculate(int id, [Body] FruitingBlockInnoculateRequest request)
        {
            return await this.refitClient.Innoculate(id, request);
        }

        public async Task<FruitingBlock> ShelfPlaceFruiting(int id, [Body] FruitingBlockShelfPlaceRequest request)
        {
            return await this.refitClient.ShelfPlaceFruiting(id, request);
        }

        public async Task<FruitingBlock> ShelfPlaceIncubate(int id, [Body] FruitingBlockShelfPlaceRequest request)
        {
            return await this.refitClient.ShelfPlaceIncubate(id, request);
        }

        public async Task<FruitingBlock> Get(int id)
        {
            return await this.refitClient.Get(id);
        }

        public async Task<FruitingBlock[]> GetAll()
        {
            return await this.refitClient.GetAll();
        }
    }
}