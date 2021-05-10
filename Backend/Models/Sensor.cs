namespace Solarponics.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public int SensorModuleId { get; set; }
        public SensorType Type { get; set; }
        public int Number { get; set; }
    }
}