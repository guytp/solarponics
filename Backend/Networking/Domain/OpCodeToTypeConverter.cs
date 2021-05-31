using System;
using System.Collections.Generic;
using System.Linq;
using Solarponics.Models.Messages;
using Solarponics.Networking.Abstractions;
using Solarponics.Networking.Exceptions;

namespace Solarponics.Networking.Domain
{
    public class OpCodeToTypeConverter : IOpCodeToTypeConverter
    {
        private readonly Dictionary<byte, Type> _map;

        public OpCodeToTypeConverter()
        {
            _map = new Dictionary<byte, Type>();
            var messageBaseType = typeof(MessageBase);
            var ass = messageBaseType.Assembly;
            var types = ass.GetTypes()
                .Where(t => messageBaseType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
            foreach (var type in types)
            {
                var obj = (MessageBase) Activator.CreateInstance(type);
                if (obj == null) continue;
                _map.Add(obj.OpCode, type);
            }
        }

        public Type TypeForOpCode(byte opCode)
        {
            if (!_map.ContainsKey(opCode))
            {
                throw new ClientException("urn:sp:badopcode", "Unknown opcode " + opCode);
            }
            return _map[opCode];
        }
    }
}