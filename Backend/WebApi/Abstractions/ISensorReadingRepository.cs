using System.Threading.Tasks;
using Solarponics.Models;

#pragma warning disable 1591

namespace Solarponics.WebApi.Abstractions
{
    public interface ISensorReadingRepository
    {
        Task<AggregateSensorReading[]> GetReadings(int id, AggregateTimeframe timeframe);
        Task<SensorReading> GetCurrentReading(int id);
    }
}