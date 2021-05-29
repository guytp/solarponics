using System;
using System.Collections.Generic;
using Solarponics.Models.Messages;
using Solarponics.Networking.Abstractions;

namespace Solarponics.Networking.Domain
{
    public abstract class MessageHandlerSelectorBase : IMessageHandlerSelector
    {
        private Dictionary<Type, IMessageHandler> _handlers;

        public IMessageHandler GetHandlerForMessage(IMessage message)
        {
            var requestType = message.GetType();
            return !_handlers.ContainsKey(requestType) ? null : _handlers[requestType];
        }

        protected void SetupHandlers(Dictionary<Type, IMessageHandler> handlers)
        {
            _handlers = handlers;
        }
    }
}