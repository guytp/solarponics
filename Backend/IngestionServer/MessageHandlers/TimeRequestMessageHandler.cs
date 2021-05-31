using System;
using System.Threading.Tasks;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.Exceptions;
using Solarponics.Models.Messages;
using Solarponics.Models.Messages.Ingestion;
using Solarponics.Networking.Abstractions;

namespace Solarponics.IngestionServer.MessageHandlers
{
    public class TimeRequestMessageHandler : IMessageHandler
    {
        public Task<IMessage> Handle(IMessage r, INetworkSession session)
        {
            var sensorModule = (session as IIngestionNetworkSession)?.SensorModule;
            if (sensorModule == null) throw new ClientMissingHandshakeException();

            var request = (TimeRequest) r;
            Console.WriteLine("TimeRequest from " + sensorModule.Name);

            var msg = (IMessage) new TimeResponse
            {
                Timestamp = (ulong) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds
            };
            
            return Task.FromResult(msg);
        }
    }
}