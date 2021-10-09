namespace Solarponics.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public int SensorModuleId { get; set; }
        public SensorType Type { get; set; }
        public int Number { get; set; }
        public decimal CriticalLowBelow { get; set; }
        public decimal WarningLowBelow { get; set; }
        public decimal WarningHighAbove { get; set; }
        public decimal CriticalHighAbove { get; set; }
    }
}