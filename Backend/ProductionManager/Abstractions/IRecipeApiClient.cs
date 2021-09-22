using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IRecipeApiClient
    {
        [Headers("Authorization: Bearer")]
        [Post("/recipes")]
        Task<Recipe> Add([Body]Recipe recipe);

        [Headers("Authorization: Bearer")]
        [Get("/recipes")]
        Task<Recipe[]> Get();
        
        [Headers("Authorization: Bearer")]
        [Put("/recipes/{id}")]
        Task Update(int id, [Body]Recipe recipe);
        
        [Headers("Authorization: Bearer")]
        [Delete("/recipes/{id}")]
        Task Delete(int id);
    }
}