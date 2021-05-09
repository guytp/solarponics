namespace Solarponics.Models.Messages
{
    public class ServerHandshakeResponse : MessageBase
    {
        public string Name { get; set; }
        public override byte OpCode => 0x01;
    }
}