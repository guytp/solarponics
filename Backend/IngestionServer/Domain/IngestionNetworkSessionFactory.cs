using Solarponics.IngestionServer.Net;
using Solarponics.Networking.Abstractions;

namespace Solarponics.IngestionServer.Domain
{
    public class IngestionNetworkSessionFactory : INetworkSessionFactory
    {
        private readonly IMessageHandlerSelector _messageHandlerSelector;
        private readonly IOpCodeToTypeConverter _opCodeToTypeConverter;

        public IngestionNetworkSessionFactory(IMessageHandlerSelector messageHandlerSelector,
            IOpCodeToTypeConverter opCodeToTypeConverter)
        {
            _messageHandlerSelector = messageHandlerSelector;
            _opCodeToTypeConverter = opCodeToTypeConverter;
        }

        public INetworkSession Create(INetworkServer server)
        {
            return new IngestionNetworkSession(server, _messageHandlerSelector, _opCodeToTypeConverter);
        }
    }
}