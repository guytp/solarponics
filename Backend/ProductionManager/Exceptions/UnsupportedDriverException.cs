using System;

namespace Solarponics.ProductionManager.Exceptions
{
    public class UnsupportedDriverException : Exception
    {
        public string DriverName { get; }
        public string HardwareType { get; }

        public UnsupportedDriverException(string driverName, string hardwareType)
            : base("Unsupported driver " + driverName + " for " + hardwareType)
        {
            DriverName = driverName;
            HardwareType = hardwareType;
        }
    }
}