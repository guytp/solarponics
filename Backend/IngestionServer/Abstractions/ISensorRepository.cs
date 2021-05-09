using System;
using System.Threading.Tasks;
using Solarponics.Models;

namespace Solarponics.IngestionServer.Abstractions
{
    public interface ISensorRepository
    {
        Task<SensorModule> GetSensorModule(Guid uniqueIdentifier);

        Task AddReading(int sensorId, decimal reading, DateTime time);
    }
}