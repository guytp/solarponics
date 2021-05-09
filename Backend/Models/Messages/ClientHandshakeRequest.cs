using System;

namespace Solarponics.Models.Messages
{
    public class ClientHandshakeRequest : MessageBase
    {
        public string Name { get; set; }
        public Guid UniqueIdentifier { get; set; }
        public string SerialNumber { get; set; }
        public override byte OpCode => 0x04;
    }
}