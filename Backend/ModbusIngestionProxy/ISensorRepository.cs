using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ModbusIngestionProxy
{
    public interface ISensorRepository
    {
        Task<SensorModuleModbusTcp[]> GetSensorModules();
    }
}