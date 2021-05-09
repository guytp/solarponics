using System;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.Exceptions;
using Solarponics.Models.Messages;

namespace Solarponics.IngestionServer.MessageHandlers
{
    public class NewSensorReadingMessageHandler : IMessageHandler
    {
        public IMessage Handle(IMessage r, INetworkSession session)
        {
            if (session.ClientHandshake == null) throw new ClientMissingHandshakeException();

            var request = (NewSensorReading) r;
            Console.WriteLine(
                "Sensor " + request.Type + " = " + request.Reading + " on " + session.ClientHandshake.Name);

            return new AcknowledgeResponse(r.Sequence);
        }
    }
}