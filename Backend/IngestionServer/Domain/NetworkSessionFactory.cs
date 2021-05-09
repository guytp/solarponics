using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.Net;

namespace Solarponics.IngestionServer.Domain
{
    public class NetworkSessionFactory : INetworkSessionFactory
    {
        private readonly IMessageHandlerSelector _messageHandlerSelector;
        private readonly IOpCodeToTypeConverter _opCodeToTypeConverter;

        public NetworkSessionFactory(IMessageHandlerSelector messageHandlerSelector,
            IOpCodeToTypeConverter opCodeToTypeConverter)
        {
            _messageHandlerSelector = messageHandlerSelector;
            _opCodeToTypeConverter = opCodeToTypeConverter;
        }

        public INetworkSession Create(INetworkServer server)
        {
            return new NetworkSession(server, _messageHandlerSelector, _opCodeToTypeConverter);
        }
    }
}