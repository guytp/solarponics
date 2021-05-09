namespace Solarponics.IngestionServer.Abstractions
{
    public interface INetworkServer
    {
        bool Start();
        bool Stop();
    }
}