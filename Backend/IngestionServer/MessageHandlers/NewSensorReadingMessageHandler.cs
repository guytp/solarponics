using System;
using System.Linq;
using System.Threading.Tasks;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.Exceptions;
using Solarponics.Models.Messages;

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
            if (session.SensorModule == null) throw new ClientMissingHandshakeException();

            var request = (NewSensorReading) r;
            Console.WriteLine(
                "Sensor " + request.Type + " = " + request.Reading + " on " + session.SensorModule.Name);

            var sensor = session.SensorModule.Sensors.FirstOrDefault(s => s.Number == request.Number && s.Type == request.Type);
            if (sensor == null)
                throw new SensorNotFoundException(request.Type, request.Number);

            await _sensorRepository.AddReading(sensor.Id, request.Reading, request.Time);

            return new AcknowledgeResponse(r.Sequence);
        }
    }
}