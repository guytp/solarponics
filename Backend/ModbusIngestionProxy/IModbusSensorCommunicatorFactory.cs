using Solarponics.Models;

namespace Solarponics.ModbusIngestionProxy
{
    public interface IModbusSensorCommunicatorFactory
    {
        IModbusSensorCommunicator Create(SensorModuleModbusTcp sensorModule);
    }
}