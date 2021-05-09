using Solarponics.Models;

namespace Solarponics.IngestionServer.Abstractions
{
    public interface INetworkSession
    {
        SensorModule SensorModule { get; set; }
    }
}