using System;
using Solarponics.Models.Messages;
using Solarponics.Server.Exceptions;

namespace Solarponics.Server.Handlers
{
    public class NewSensorReadingHandler : IMessageHandler
    {
        public MessageBase Handle(MessageBase r, CommandSession session)
        {
            if (session.ClientHandshake == null)
            {
                throw new ClientMissingHandshakeException();
            }

            var request = (NewSensorReading)r;
            Console.WriteLine("Sensor " + request.Type + " = " + request.Reading + " on " + session.ClientHandshake.Name);

            return new AcknowledgeResponse(r.Sequence);
        }
    }
}