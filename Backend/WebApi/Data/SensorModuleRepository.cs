using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class SensorModuleRepository : ISensorModuleRepository
    {
        private const string ProcedureNameSensorModuleGetAll =
            "[dbo].[SensorModuleGetAll]";
        private const string ProcedureNameAddModbusTcp =
            "[dbo].[SensorModuleAddModbusTcp]";
        private const string ProcedureNameGetModbusTcp =
            "[dbo].[SensorModuleGetModbusTcp]";
        private const string ProcedureNameDelete =
            "[dbo].[SensorModuleDelete]";

        public SensorModuleRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<SensorModule[]> GetAll()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameSensorModuleGetAll, null);
            await storedProcedure.ExecuteReaderAsync();
            var modules = (await storedProcedure.GetDataSetAsync<SensorModule>())?.ToArray();
            if (modules == null)
                return null;

            var sensors = (await storedProcedure.GetDataSetAsync<Sensor>()).ToArray();
            foreach (var module in modules)
            {
                module.Sensors = sensors.Where(s => s.SensorModuleId == module.Id).ToArray();
                if (module.Sensors.Length == 0)
                    module.Sensors = null;
            }

            return modules;
        }

        public async Task<SensorModuleModbusTcp[]> GetModbusTcp()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGetModbusTcp, null);
            await storedProcedure.ExecuteReaderAsync();
            var modules = (await storedProcedure.GetDataSetAsync<SensorModuleModbusTcp>())?.ToArray();
            if (modules == null)
                return new SensorModuleModbusTcp[0];

            var sensors = await storedProcedure.GetDataSetAsync<Sensor>();
            foreach (var module in modules)
                module.Sensors = sensors.Where(s => s.SensorModuleId == module.Id).ToArray();

            return modules;
        }

        public async Task Delete(int id)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameDelete, new[] {
                new StoredProcedureParameter("@id", id)
            });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task<int> AddModbusTcp(int roomId, string serialNumber, string name, string ipAddress, ushort port, int userId, int? temperatureSensorNumber, decimal? temperatureWarningLowBelow, decimal? temperatureCriticalLowBelow, decimal? temperatureWarningHighAbove, decimal? temperatureCriticalHighAbove, int? humiditySensorNumber, decimal? humidityWarningLowBelow, decimal? humidityCriticalLowBelow, decimal?  humidityWarningHighAbove, decimal? humidityCriticalHighAbove, int? carbonDioxideSensorNumber, decimal? carbonDioxideWarningLowBelow, decimal? carbonDioxideCriticalLowBelow, decimal? carbonDioxideWarningHighAbove, decimal? carbonDioxideCriticalHighAbove)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameAddModbusTcp, new[] {
                new StoredProcedureParameter("@roomId", roomId),
                new StoredProcedureParameter("@serialNumber", serialNumber),
                new StoredProcedureParameter("@name", name),
                new StoredProcedureParameter("@ipAddress", ipAddress),
                new StoredProcedureParameter("@port", (short)port),
                new StoredProcedureParameter("@userId", userId),
                new StoredProcedureParameter("@temperatureSensorNumber", temperatureSensorNumber),
                new StoredProcedureParameter("@temperatureWarningLowBelow", temperatureWarningLowBelow),
                new StoredProcedureParameter("@temperatureCriticalLowBelow", temperatureCriticalLowBelow),
                new StoredProcedureParameter("@temperatureCriticalHighAbove", temperatureCriticalHighAbove),
                new StoredProcedureParameter("@temperatureWarningHighAbove", temperatureWarningHighAbove),
                new StoredProcedureParameter("@humiditySensorNumber", humiditySensorNumber),
                new StoredProcedureParameter("@humidityWarningLowBelow", humidityWarningLowBelow),
                new StoredProcedureParameter("@humidityCriticalLowBelow", humidityCriticalLowBelow),
                new StoredProcedureParameter("@humidityCriticalHighAbove", humidityCriticalHighAbove),
                new StoredProcedureParameter("@humidityWarningHighAbove", humidityWarningHighAbove),
                new StoredProcedureParameter("@carbonDioxideSensorNumber", carbonDioxideSensorNumber),
                new StoredProcedureParameter("@carbonDioxideWarningLowBelow", carbonDioxideWarningLowBelow),
                new StoredProcedureParameter("@carbonDioxideCriticalLowBelow", carbonDioxideCriticalLowBelow),
                new StoredProcedureParameter("@carbonDioxideCriticalHighAbove", carbonDioxideCriticalHighAbove),
                new StoredProcedureParameter("@carbonDioxideWarningHighAbove", carbonDioxideWarningHighAbove)
            });
            return await storedProcedure.ExecuteScalarAsync<int>();
        }
    }
}