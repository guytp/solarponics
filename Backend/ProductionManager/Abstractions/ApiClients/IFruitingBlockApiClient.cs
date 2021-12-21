using Refit;
using Solarponics.Models;
using Solarponics.Models.WebApi;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Abstractions.ApiClients
{
    public interface IFruitingBlockApiClient
    {
        [Headers("Authorization: Bearer")]
        [Put("/fruiting-blocks")]
        Task<FruitingBlock> Add([Body] FruitingBlockAddRequest request);

        [Headers("Authorization: Bearer")]
        [Post("/fruiting-blocks/by-id/{id}/innoculate")]
        Task<FruitingBlock> Innoculate(int id, [Body] FruitingBlockInnoculateRequest request);

        [Headers("Authorization: Bearer")]
        [Post("/fruiting-blocks/by-id/{id}/shelf/incubate")]
        Task<FruitingBlock> ShelfPlaceIncubate(int id, [Body] FruitingBlockShelfPlaceRequest request);

        [Headers("Authorization: Bearer")]
        [Post("/fruiting-blocks/by-id/{id}/shelf/fruiting")]
        Task<FruitingBlock> ShelfPlaceFruiting(int id, [Body] FruitingBlockShelfPlaceRequest request);

        [Headers("Authorization: Bearer")]
        [Get("/fruiting-blocks/by-id/{id}")]
        Task<FruitingBlock> Get(int id);

        [Headers("Authorization: Bearer")]
        [Get("/fruiting-blocks")]
        Task<FruitingBlock[]> GetAll();
    }
}