using System.Threading.Tasks;
using Solarponics.Models;

namespace Solarponics.Networking.Abstractions
{
    public interface IProvisioningRepository
    {
        Task<SensorModuleConfig> GetConfig(string serialNumber);
        Task Provision(string serialNumber);
    }
}