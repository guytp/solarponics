using Refit;
using Solarponics.Models;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Core.Abstractions.ApiClients
{
    public interface ISensorModuleApiClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/sensor-modules")]
        Task<SensorModule[]> Get();

        [Headers("Authorization: Bearer")]
        [Get("/sensor-modules/by-type/modbus-tcp")]
        Task<SensorModuleModbusTcp[]> GetModbusTcp();
        
        [Headers("Authorization: Bearer")]
        [Delete("/sensor-modules/by-id/{id}")]
        Task Delete(int id);

        [Headers("Authorization: Bearer")]
        [Put("/sensor-modules/by-type/modbus-tcp")]
        Task<int> AddModbusTcp([Body]SensorModuleModbusTcp sensorModule);
    }
}