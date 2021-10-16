using System.Threading.Tasks;
using Solarponics.Models;

#pragma warning disable 1591

namespace Solarponics.WebApi.Abstractions
{
    public interface ISensorModuleRepository
    {
        Task<SensorModule[]> GetAll();
        Task<SensorModuleModbusTcp[]> GetModbusTcp();
        Task Delete(int id);
        Task<int> AddModbusTcp(int roomId, string serialNumber, string name, string ipAddress, ushort port, int userId, int? temperatureSensorNumber, decimal? temperatureWarningLowBelow, decimal? temperatureCriticalLowBelow, decimal? temperatureWarningHighAbove, decimal? temperatureCriticalHighAbove, int? humiditySensorNumber, decimal? humidityWarningLowBelow, decimal? humidityCriticalLowBelow, decimal? humidityWarningHighAbove, decimal? humidityCriticalHighAbove, int? carbonDioxideSensorNumber, decimal? carbonDioxideWarningLowBelow, decimal? carbonDioxideCriticalLowBelow, decimal? carbonDioxideWarningHighAbove, decimal? carbonDioxideCriticalHighAbove);
    }
}