using System;
using System.Collections.Generic;
using Solarponics.Models.Messages.Provisioning;
using Solarponics.Networking.Abstractions;
using Solarponics.Networking.Domain;
using Solarponics.ProvisioningServer.MessageHandlers;

namespace Solarponics.ProvisioningServer.Domain
{
    public class ProvisioningMessageHandlerSelector : MessageHandlerSelectorBase
    {
        public ProvisioningMessageHandlerSelector(IProvisioningRepository provisioningRepository)
        {
            SetupHandlers(new Dictionary<Type, IMessageHandler>
            {
                {typeof(ProvisioningRequest), new ProvisioningRequestMessageHandler(provisioningRepository)}
            });
        }
    }
}