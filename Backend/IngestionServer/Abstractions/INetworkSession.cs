using Solarponics.Models.Messages;

namespace Solarponics.IngestionServer.Abstractions
{
    public interface INetworkSession
    {
        ClientHandshakeRequest ClientHandshake { get; set; }
    }
}