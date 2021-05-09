using System;
using System.Collections.Generic;
using Solarponics.Models.Messages;

namespace Solarponics.Server.Handlers
{
    public class MessageHandler
    {
        private readonly Dictionary<Type, IMessageHandler> _handlers;

        public MessageHandler()
        {
            this._handlers = new Dictionary<Type, IMessageHandler>();
            _handlers.Add(typeof(NewSensorReading), new NewSensorReadingHandler());
            _handlers.Add(typeof(ClientHandshakeRequest), new ClientHandshakeRequestHandler());
        }

        public MessageBase HandleMessage(MessageBase request, CommandSession session)
        {
            var requestType = request.GetType();
            if (!this._handlers.ContainsKey(requestType))
            {
                throw new NotImplementedException();
            }

            return this._handlers[requestType].Handle(request, session);
        }
    }
}