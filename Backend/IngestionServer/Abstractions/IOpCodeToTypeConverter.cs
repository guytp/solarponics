using System;

namespace Solarponics.IngestionServer.Abstractions
{
    public interface IOpCodeToTypeConverter
    {
        Type TypeForOpCode(byte opCode);
    }
}