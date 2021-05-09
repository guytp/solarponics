using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Solarponics.Models.Messages;

namespace Solarponics.Server
{
    public static class OpCodeToTypeConverter
    {
        private static readonly Dictionary<byte, Type> map;

        static OpCodeToTypeConverter()
        {
            map = new Dictionary<byte, Type>();
            var messageBaseType = typeof(MessageBase);
            var ass = messageBaseType.Assembly;
            var types = ass.GetTypes()
                .Where(t => messageBaseType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
            foreach (var type in types)
            {
                var obj = (MessageBase) Activator.CreateInstance(type);
                if (obj == null) continue;
                map.Add(obj.OpCode, type);
            }
        }

        public static Type TypeForOpCode(byte opCode)
        {
            return map[opCode];
        }
    }
}