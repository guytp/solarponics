using System;
using System.Collections.Generic;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.MessageHandlers;
using Solarponics.Models.Messages;

namespace Solarponics.IngestionServer.Domain
{
    public class MessageHandlerSelector : IMessageHandlerSelector
    {
        private readonly Dictionary<Type, IMessageHandler> _handlers;

        public MessageHandlerSelector()
        {
            _handlers = new Dictionary<Type, IMessageHandler>
            {
                {typeof(NewSensorReading), new NewSensorReadingMessageHandler()},
                {typeof(ClientHandshakeRequest), new ClientHandshakeRequestMessageHandler()}
            };
        }

        public IMessageHandler GetHandlerForMessage(IMessage message)
        {
            var requestType = message.GetType();
            return !_handlers.ContainsKey(requestType) ? null : _handlers[requestType];
        }
    }
}