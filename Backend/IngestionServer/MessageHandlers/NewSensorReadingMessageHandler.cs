using System;
using System.Linq;
using System.Threading.Tasks;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.Exceptions;
using Solarponics.Models.Messages;
using Solarponics.Models.Messages.Ingestion;
using Solarponics.Networking.Abstractions;

namespace Solarponics.IngestionServer.MessageHandlers
{
    public class NewSensorReadingMessageHandler : IMessageHandler
    {
        private readonly ISensorRepository _sensorRepository;

        public NewSensorReadingMessageHandler(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        public async Task<IMessage> Handle(IMessage r, INetworkSession session)
        {
            var sensorModule = (session as IIngestionNetworkSession)?.SensorModule;
            if (sensorModule == null) throw new ClientMissingHandshakeException();

            var request = (NewSensorReading) r;
            Console.WriteLine(
                "Sensor " + request.Type + " = " + request.Reading + " on " + sensorModule.Name);

            var sensor = sensorModule.Sensors.FirstOrDefault(s => s.Number == request.Number && s.Type == request.Type);
            if (sensor == null)
                throw new SensorNotFoundException(request.Type, request.Number);

            var time = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(request.Timestamp);
            await _sensorRepository.AddReading(sensor.Id, request.Reading, time);

            return new AcknowledgeResponse(r.Sequence);
        }
    }
}