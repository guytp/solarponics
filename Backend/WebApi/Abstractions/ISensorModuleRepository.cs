using System.Threading.Tasks;
using Solarponics.Models;

#pragma warning disable 1591

namespace Solarponics.WebApi.Abstractions
{
    public interface ISensorModuleRepository
    {
        Task<SensorModule[]> GetAll();
        Task<SensorModuleConfig[]> ProvisionQueueGetAll();
        Task ProvisioningQueueAdd(SensorModuleConfig config);
        Task<SensorModuleConfig> ProvisionQueueGetBySerialNumber(string serialNumber);
        Task ProvisioningQueueDelete(string serialNumber);
    }
}