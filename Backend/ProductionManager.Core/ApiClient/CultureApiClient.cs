using Refit;
using Solarponics.Models;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.ApiClient
{
    public class CultureApiClient : ICultureApiClient
    {
        private readonly ICultureApiClient refitClient;

        public CultureApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<ICultureApiClient>(settings.ApiBaseUrl, refitSettings);
        }
        
        public async Task<Culture> BookIn(CultureBookInRequest request)
        {
            return await this.refitClient.BookIn(request);
        }

        public async Task<Culture> CreateFromRecipe([Body] CultureCreateFromReciptRequest request)
        {
            return await this.refitClient.CreateFromRecipe(request);

        }

        public async Task<Culture> Innoculate([Body] CultureInnoculateRequest request)
        {
            return await this.refitClient.Innoculate(request);
        }

        public async Task<Culture> Get(int id)
        {
            return await this.refitClient.Get(id);
        }

        public async Task<Culture[]> GetAll()
        {
            return await this.refitClient.GetAll();
        }
    }
}