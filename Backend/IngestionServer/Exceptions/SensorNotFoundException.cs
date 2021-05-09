using System;
using Solarponics.Models;

namespace Solarponics.IngestionServer.Exceptions
{
    public class SensorNotFoundException : Exception
    {
        public SensorNotFoundException(SensorType sensorType, byte number)
            : base("Sensor " + sensorType + "/ " + number + " not found in sensor module")
        {
            SensorType = sensorType;
            Number = number;
        }

        public SensorType SensorType { get; }
        public byte Number { get; }
    }
}