namespace Solarponics.Models
{
    public abstract class SerialDeviceSettings : IDriverSettings
    {
        public string DriverName { get; set; }

        public string SerialPort { get; set; }
        
        public int BaudRate { get; set; }
        
        public Parity Parity { get; set; }
            
        public int DataBits { get; set; }

        public StopBits StopBits { get; set; }
    }
}