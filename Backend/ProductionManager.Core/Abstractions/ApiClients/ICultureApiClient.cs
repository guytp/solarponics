using Refit;
using Solarponics.Models;
using Solarponics.Models.WebApi;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.Abstractions.ApiClients
{
    public interface ICultureApiClient
    {
        [Headers("Authorization: Bearer")]
        [Put("/cultures/book-in")]
        Task<Culture> BookIn([Body] CultureBookInRequest request);

        [Headers("Authorization: Bearer")]
        [Put("/cultures/from-recipe")]
        Task<Culture> CreateFromRecipe([Body] CultureCreateFromReciptRequest request);

        [Headers("Authorization: Bearer")]
        [Post("/cultures/innoculate")]
        Task<Culture> Innoculate([Body] CultureInnoculateRequest request);

        [Headers("Authorization: Bearer")]
        [Get("/cultures/by-id/{id}")]
        Task<Culture> Get(int id);

        [Headers("Authorization: Bearer")]
        [Get("/cultures")]
        Task<Culture[]> GetAll();
    }
}