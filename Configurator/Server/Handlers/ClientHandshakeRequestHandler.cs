using System;
using Solarponics.Models.Messages;

namespace Solarponics.Server.Handlers
{
    public class ClientHandshakeRequestHandler : IMessageHandler
    {
        public MessageBase Handle(MessageBase inbound, CommandSession session)
        {
            if (session.ClientHandshake != null)
            {
                throw new Exception("Already performed handshake with client");
            }
            
            var request = (ClientHandshakeRequest) inbound;
            session.ClientHandshake = request;

            return new ServerHandshakeResponse
            {
                Name = Environment.MachineName
            };
        }
    }
}