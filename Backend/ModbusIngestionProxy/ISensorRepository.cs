using System.Threading.Tasks;

namespace Solarponics.ModbusIngestionProxy
{
    public interface ISensorRepository
    {
        Task<SensorModuleWithModbusFields[]> GetSensorModules();
    }
}