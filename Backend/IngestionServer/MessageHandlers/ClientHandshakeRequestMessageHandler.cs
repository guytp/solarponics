using System;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.Models.Messages;

namespace Solarponics.IngestionServer.MessageHandlers
{
    public class ClientHandshakeRequestMessageHandler : IMessageHandler
    {
        public IMessage Handle(IMessage inbound, INetworkSession session)
        {
            if (session.ClientHandshake != null) throw new Exception("Already performed handshake with client");

            var request = (ClientHandshakeRequest) inbound;
            session.ClientHandshake = request;

            return new ServerHandshakeResponse
            {
                Name = Environment.MachineName
            };
        }
    }
}