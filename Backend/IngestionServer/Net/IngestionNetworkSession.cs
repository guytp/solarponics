using Solarponics.IngestionServer.Abstractions;
using Solarponics.Models;
using Solarponics.Networking;
using Solarponics.Networking.Abstractions;

namespace Solarponics.IngestionServer.Net
{
    public class IngestionNetworkSession : NetworkSessionBase, IIngestionNetworkSession
    {
        public IngestionNetworkSession(INetworkServer server, IMessageHandlerSelector messageHandlerSelector,
            IOpCodeToTypeConverter opCodeToTypeConverter) : base(server, messageHandlerSelector, opCodeToTypeConverter)
        {
        }

        public SensorModule SensorModule { get; set; }
    }
}