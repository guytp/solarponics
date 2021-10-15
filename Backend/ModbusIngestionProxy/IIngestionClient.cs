using Solarponics.Models;

namespace Solarponics.ModbusIngestionProxy
{
    public interface IIngestionClient
    {
        bool IsConnectedAndStarted { get; }
        void Start(string serialNumber);
        void SendReading(SensorType type, byte number, decimal reading);
    }
}