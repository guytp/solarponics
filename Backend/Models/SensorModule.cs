using System;

namespace Solarponics.Models
{
    public class SensorModule
    {
        public string Room { get; set; }
        public string Location { get; set; }
        public int Id { get; set; }
        public Sensor[] Sensors { get; set; }
        public Guid UniqueIdentifier { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
    }
}