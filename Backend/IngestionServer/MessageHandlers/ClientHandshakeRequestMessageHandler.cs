using System;
using System.Threading.Tasks;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.Exceptions;
using Solarponics.Models.Messages;

namespace Solarponics.IngestionServer.MessageHandlers
{
    public class ClientHandshakeRequestMessageHandler : IMessageHandler
    {
        private readonly ISensorRepository _sensorRepository;

        public ClientHandshakeRequestMessageHandler(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        public async Task<IMessage> Handle(IMessage inbound, INetworkSession session)
        {
            if (session.SensorModule != null) throw new Exception("Already performed handshake with client");

            var request = (ClientHandshakeRequest) inbound;

            var sensorModule = await _sensorRepository.GetSensorModule(request.UniqueIdentifier);
            if (sensorModule == null || sensorModule.Name != request.Name || sensorModule.SerialNumber != request.SerialNumber)
                throw new SensorModuleNotFoundException();

            session.SensorModule = sensorModule;

            return new ServerHandshakeResponse
            {
                Name = Environment.MachineName
            };
        }
    }
}