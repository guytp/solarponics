using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Abstractions.ApiClients
{
    public interface ILocationApiClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/locations")]
        Task<Location[]> Get();

        [Headers("Authorization: Bearer")]
        [Put("/locations")]
        Task<int> Add([Body]Location location);

        [Headers("Authorization: Bearer")]
        [Put("/locations/by-id/{room.locationId}/rooms")]
        Task<int> AddRoom([Body]Room room);
    }
}