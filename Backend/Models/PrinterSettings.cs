using System.ComponentModel;

namespace Solarponics.Models
{
    public abstract class PrinterSettings
    {
        public string DriverName { get; set; }
        public string QueueName { get; set; }
    }
}