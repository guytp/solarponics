using Solarponics.Models;

namespace Solarponics.ModbusIngestionProxy
{
    public interface IModbusSensorCommunicatorFactory
    {
        IModbusSensorCommunicator Create(SensorModuleWithModbusFields sensorModule);
    }
}