using System;
using System.Threading.Tasks;

namespace Solarponics.ModbusIngestionProxy
{
    public interface IModbusSensorCommunicator
    {
        SensorModuleWithModbusFields SensorModule { get; }

        Task<SensorModuleReading> GetReadings();

        void SubmitIngestionReading(SensorModuleReading result);

        DateTime? LastReadingsSubmitted { get; }
    }
}