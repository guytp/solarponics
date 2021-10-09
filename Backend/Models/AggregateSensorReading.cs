using System;

namespace Solarponics.Models
{
    public class AggregateSensorReading
    {
        public decimal Minimum { get; set; }
        public decimal Maximum { get; set; }
        public decimal Average { get; set; }
        public DateTime Time { get; set; }
    }
}