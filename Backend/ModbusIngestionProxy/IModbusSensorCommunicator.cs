using Solarponics.Models;
using System;
using System.Threading.Tasks;

namespace Solarponics.ModbusIngestionProxy
{
    public interface IModbusSensorCommunicator
    {
        SensorModuleModbusTcp SensorModule { get; }

        Task<SensorModuleReading> GetReadings();

        void SubmitIngestionReading(SensorModuleReading result);

        DateTime? LastReadingsSubmitted { get; }
    }
}