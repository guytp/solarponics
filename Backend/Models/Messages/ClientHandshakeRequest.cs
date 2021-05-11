using System;

namespace Solarponics.Models.Messages
{
    public class ClientHandshakeRequest : MessageBase
    {
        public string SerialNumber { get; set; }
        public override byte OpCode => 0x04;
    }
}