using System;

namespace Solarponics.Models.Messages.Ingestion
{
    public class NewSensorReading : MessageBase
    {
        public override byte OpCode => 0x03;

        public decimal Reading { get; set; }

        public SensorType Type { get; set; }

        public byte Number { get; set; }

        public DateTime Time { get; set; }
    }
}