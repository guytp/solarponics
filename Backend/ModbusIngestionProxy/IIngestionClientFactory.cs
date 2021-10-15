namespace Solarponics.ModbusIngestionProxy
{
    public interface IIngestionClientFactory
    {
        IIngestionClient Create();
    }
}