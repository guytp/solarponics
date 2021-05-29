using System;
using System.Collections.Generic;
using Solarponics.Networking.Abstractions;
using Solarponics.Networking.Domain;

namespace Solarponics.ProvisioningServer.Domain
{
    public class ProvisioningMessageHandlerSelector : MessageHandlerSelectorBase
    {
        public ProvisioningMessageHandlerSelector()
        {
            this.SetupHandlers(new Dictionary<Type, IMessageHandler>
            {
                //{ typeof(NewSensorReading), new NewSensorReadingMessageHandler(sensorRepository) },
            });
        }
    }
}