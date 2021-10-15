using System.Threading.Tasks;

namespace Solarponics.ModbusIngestionProxy
{
    public interface ISensorModuleProvider
    {
        Task<IModbusSensorCommunicator[]> GetModulesRequiringRead(int maximumCount, IModbusSensorCommunicator[] excludeCommunicators);
    }
}