using Solarponics.Models;
using Solarponics.Networking.Abstractions;

namespace Solarponics.IngestionServer.Abstractions
{
    public interface IIngestionNetworkSession : INetworkSession
    {
        SensorModule SensorModule { get; set; }
    }
}