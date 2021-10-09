using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Abstractions.ApiClients
{
    public interface ISensorModuleApiClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/sensor-modules")]
        Task<SensorModule[]> Get();
    }
}