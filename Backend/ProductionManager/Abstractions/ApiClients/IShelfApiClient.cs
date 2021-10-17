using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Abstractions.ApiClients
{
    public interface IShelfApiClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/shelves")]
        Task<Shelf[]> Get();


        [Headers("Authorization: Bearer")]
        [Put("/shelves")]
        Task<int> Add(Shelf shelf);

        [Headers("Authorization: Bearer")]
        [Delete("/shelves/by-id/{id}")]
        Task Delete(int id);
    }
}