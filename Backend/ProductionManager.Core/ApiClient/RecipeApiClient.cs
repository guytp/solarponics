using Refit;
using Solarponics.Models;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.Data;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.ApiClient
{
    public class RecipeApiClient : IRecipeApiClient
    {
        private readonly IRecipeApiClient refitClient;

        public RecipeApiClient(ProductionManagerSettings settings, IAuthenticationSession authenticationSession)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authenticationSession.Token?.Token)
            };
            refitClient = RestService.For<IRecipeApiClient>(settings.ApiBaseUrl, refitSettings);
        }
        
        public Task<Recipe> Add(Recipe recipe)
        {
            return this.refitClient.Add(recipe);
        }

        public Task<Recipe[]> Get()
        {
            return this.refitClient.Get();
        }
        
        public Task Update(int id, Recipe recipe)
        {
            return this.refitClient.Update(id, recipe);
        }
        
        public Task Delete(int id)
        {
            return this.refitClient.Delete(id);
        }
    }
}