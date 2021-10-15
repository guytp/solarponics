namespace Solarponics.ModbusIngestionProxy
{
    public class SensorModuleReading
    {
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set;  }
        public decimal? CarbonDioxide { get; set; }
    }
}