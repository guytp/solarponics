using Solarponics.Networking;
using Solarponics.Networking.Abstractions;

namespace Solarponics.ProvisioningServer.Net
{
    public class ProvisioningNetworkSession : NetworkSessionBase
    {
        public ProvisioningNetworkSession(INetworkServer server, IMessageHandlerSelector messageHandlerSelector,
            IOpCodeToTypeConverter opCodeToTypeConverter) : base(server, messageHandlerSelector, opCodeToTypeConverter)
        {
        }
    }
}