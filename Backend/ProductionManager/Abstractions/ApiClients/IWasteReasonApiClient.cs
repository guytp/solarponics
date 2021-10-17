using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Abstractions.ApiClients
{
    public interface IWasteReasonApiClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/waste-reasons")]
        Task<WasteReason[]> Get();


        [Headers("Authorization: Bearer")]
        [Put("/waste-reasons")]
        Task<int> Add(WasteReason wasteReason);

        [Headers("Authorization: Bearer")]
        [Delete("/waste-reasons/by-id/{id}")]
        Task Delete(int id);
    }
}