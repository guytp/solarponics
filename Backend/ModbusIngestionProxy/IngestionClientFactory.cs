using Microsoft.Extensions.Configuration;

namespace Solarponics.ModbusIngestionProxy
{
    public class IngestionClientFactory : IIngestionClientFactory
    {
        private readonly IConfiguration configuration;
        private readonly ICommandClientFactory factory;
        public IngestionClientFactory(IConfiguration configuration, ICommandClientFactory factory)
        {
            this.configuration = configuration;
            this.factory = factory;
        }

        public IIngestionClient Create()
        {
            return new IngestionClient(configuration, factory);
        }
    }
}
