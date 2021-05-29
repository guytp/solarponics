using System;

namespace Solarponics.Networking.Abstractions
{
    public interface IOpCodeToTypeConverter
    {
        Type TypeForOpCode(byte opCode);
    }
}