using System;

namespace Solarponics.IngestionServer.Exceptions
{
    public class SensorModuleNotFoundException : Exception
    {
        public SensorModuleNotFoundException()
            : base("Sensor module was not found")
        {
        }
    }
}