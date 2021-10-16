using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;

namespace Solarponics.ModbusIngestionProxy
{
    public class SensorRepository : ISensorRepository
    {
        private const string ProcedureNameGet =
            "[dbo].[SensorModuleGetModbusTcp]";

        public SensorRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<SensorModuleModbusTcp[]> GetSensorModules()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, null);
            await storedProcedure.ExecuteReaderAsync();
            var modules = (await storedProcedure.GetDataSetAsync<SensorModuleModbusTcp>())?.ToArray();
            if (modules == null)
                return new SensorModuleModbusTcp[0];

            var sensors = await storedProcedure.GetDataSetAsync<Sensor>();
            foreach (var module in modules)
                module.Sensors = sensors.Where(s => s.SensorModuleId == module.Id).ToArray();

            return modules;
        }
    }
}