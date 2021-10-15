namespace Solarponics.ModbusIngestionProxy
{
    public interface ICommandClientFactory
    {
        ICommandClient Create(string ipAddress, short port);
    }
}