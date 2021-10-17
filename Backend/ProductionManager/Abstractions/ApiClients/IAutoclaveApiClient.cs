using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Abstractions.ApiClients
{
    public interface IAutoclaveApiClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/autoclaves")]
        Task<Autoclave[]> Get();


        [Headers("Authorization: Bearer")]
        [Put("/autoclaves")]
        Task<int> Add(Autoclave autoclave);

        [Headers("Authorization: Bearer")]
        [Delete("/autoclaves/by-id/{id}")]
        Task Delete(int id);
    }
}