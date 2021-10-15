namespace Solarponics.ModbusIngestionProxy
{
    public class CommandClientFactory : ICommandClientFactory
    {
        public ICommandClient Create(string ipAddress, short port)
        {
            return new CommandClient(ipAddress, port);
        }
    }
}