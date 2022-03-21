using Refit;
using Solarponics.Models;
using Solarponics.Models.WebApi;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.Abstractions.ApiClients
{
    public interface IGrainSpawnApiClient
    {
        [Headers("Authorization: Bearer")]
        [Put("/grain-spawn")]
        Task<GrainSpawn> Add([Body] GrainSpawnAddRequest request);

        [Headers("Authorization: Bearer")]
        [Post("/grain-spawn/by-id/{id}/innoculate")]
        Task<GrainSpawn> Innoculate(int id, [Body] GrainSpawnInnoculateRequest request);

        [Headers("Authorization: Bearer")]
        [Post("/grain-spawn/by-id/{id}/mix")]
        Task Mix(int id, [Body] GrainSpawnAddMixRequest request);

        [Headers("Authorization: Bearer")]
        [Post("/grain-spawn/by-id/{id}/shelf")]
        Task<GrainSpawn> ShelfPlace(int id, [Body] GrainSpawnShelfPlaceRequest request);

        [Headers("Authorization: Bearer")]
        [Get("/grain-spawn/by-id/{id}")]
        Task<GrainSpawn> Get(int id);

        [Headers("Authorization: Bearer")]
        [Get("/grain-spawn")]
        Task<GrainSpawn[]> GetAll();
    }
}