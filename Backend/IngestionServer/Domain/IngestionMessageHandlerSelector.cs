using System;
using System.Collections.Generic;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.MessageHandlers;
using Solarponics.Models.Messages.Ingestion;
using Solarponics.Networking.Abstractions;
using Solarponics.Networking.Domain;

namespace Solarponics.IngestionServer.Domain
{
    public class IngestionMessageHandlerSelector : MessageHandlerSelectorBase
    {
        public IngestionMessageHandlerSelector(ISensorRepository sensorRepository)
        {
            SetupHandlers(new Dictionary<Type, IMessageHandler>
            {
                {typeof(NewSensorReading), new NewSensorReadingMessageHandler(sensorRepository)},
                {typeof(ClientHandshakeRequest), new ClientHandshakeRequestMessageHandler(sensorRepository)},
                {typeof(TimeRequest), new TimeRequestMessageHandler()}
            });
        }
    }
}