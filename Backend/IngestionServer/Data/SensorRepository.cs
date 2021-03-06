using System;
using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.Models;

namespace Solarponics.IngestionServer.Data
{
    public class SensorRepository : ISensorRepository
    {
        private const string ProcedureNameSensorModuleGetBySerialNumber =
            "[dbo].[SensorModuleGetBySerialNumber]";

        private const string ProcedureNameReadingAdd = "[dbo].[ReadingAdd]";

        public SensorRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<SensorModule> GetSensorModule(string serialNumber)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(
                ProcedureNameSensorModuleGetBySerialNumber,
                new[]
                {
                    new StoredProcedureParameter("serialNumber", serialNumber)
                });
            await storedProcedure.ExecuteReaderAsync();
            var module = await storedProcedure.GetFirstOrDefaultRowAsync<SensorModule>();
            if (module == null)
                return null;

            var sensors = await storedProcedure.GetDataSetAsync<Sensor>();
            module.Sensors = sensors.ToArray();
            return module;
        }

        public async Task AddReading(int sensorId, decimal reading, DateTime time)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(
                ProcedureNameReadingAdd,
                new[]
                {
                    new StoredProcedureParameter("sensorId", sensorId),
                    new StoredProcedureParameter("reading", reading),
                    new StoredProcedureParameter("time", time)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }
    }
}