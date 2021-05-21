namespace Solarponics.Models
{
    public class SensorModuleConfig
    {
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Room { get; set; }
        public SensorModuleSensorConfig SensorConfig { get; set; }
        public NetworkType NetworkType { get; set; }
        public WirelessConfig WirelessConfig { get; set; }
        public IpConfigType IpConfigType { get; set; }
        public IpConfig StaticIpConfig { get; set; }
        public string ServerAddress { get; set; }
    }
}