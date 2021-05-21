namespace Solarponics.Models
{
    public class SensorModuleConfigRow
    {
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Room { get; set; }
        public byte NumberTemperatureSensors { get; set; }
        public byte NumberHumiditySensors { get; set; }
        public byte NumberCarbonDioxideSensors { get; set; }
        public byte NetworkType { get; set; }
        public string WirelessSsid { get; set; }
        public string WirelessKey { get; set; }
        public byte IpType { get; set; }
        public string IpAddress { get; set; }
        public string IpBrodcast { get; set; }
        public string IpGateway { get; set; }
        public string IpDns { get; set; }
        public string ServerAddress { get; set; }
    }
}