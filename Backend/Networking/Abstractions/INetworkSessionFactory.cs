namespace Solarponics.Networking.Abstractions
{
    public interface INetworkSessionFactory
    {
        INetworkSession Create(INetworkServer server);
    }
}