using Solarponics.Networking.Abstractions;
using Solarponics.ProvisioningServer.Net;

namespace Solarponics.ProvisioningServer.Domain
{
    public class ProvisioningNetworkSessionFactory : INetworkSessionFactory
    {
        private readonly IMessageHandlerSelector _messageHandlerSelector;
        private readonly IOpCodeToTypeConverter _opCodeToTypeConverter;

        public ProvisioningNetworkSessionFactory(IMessageHandlerSelector messageHandlerSelector,
            IOpCodeToTypeConverter opCodeToTypeConverter)
        {
            _messageHandlerSelector = messageHandlerSelector;
            _opCodeToTypeConverter = opCodeToTypeConverter;
        }

        public INetworkSession Create(INetworkServer server)
        {
            return new ProvisioningNetworkSession(server, _messageHandlerSelector, _opCodeToTypeConverter);
        }
    }
}