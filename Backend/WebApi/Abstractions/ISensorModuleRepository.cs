using System.Threading.Tasks;
using Solarponics.Models;

#pragma warning disable 1591

namespace Solarponics.WebApi.Abstractions
{
    public interface ISensorModuleRepository
    {
        Task<SensorModule[]> GetAll();
    }
}