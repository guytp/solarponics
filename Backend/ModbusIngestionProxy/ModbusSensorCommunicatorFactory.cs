namespace Solarponics.ModbusIngestionProxy
{
    public class ModbusSensorCommunicatorFactory : IModbusSensorCommunicatorFactory
    {
        private readonly IIngestionClientFactory ingestionClientFactory;
        public ModbusSensorCommunicatorFactory(IIngestionClientFactory ingestionClientFactory)
        {
            this.ingestionClientFactory = ingestionClientFactory;
        }

        public IModbusSensorCommunicator Create(SensorModuleWithModbusFields sensorModule)
        {
            return new ModbusSensorCommunicator(sensorModule, ingestionClientFactory.Create());
        }
    }
}