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
    }
}