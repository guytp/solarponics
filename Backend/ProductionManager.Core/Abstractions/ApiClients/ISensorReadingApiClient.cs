using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.Abstractions.ApiClients
{
    public interface ISensorReadingApiClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/readings/by-sensor-id/{id}/aggregate-by/{timeframe}")]
        Task<AggregateSensorReading[]> GetAggregate(int id, AggregateTimeframe timeframe);

        [Headers("Authorization: Bearer")]
        [Get("/readings/by-sensor-id/{id}")]
        Task<SensorReading> GetCurrent(int id);
    }
}