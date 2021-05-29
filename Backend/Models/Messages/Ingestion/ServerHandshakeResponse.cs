namespace Solarponics.Models.Messages.Ingestion
{
    public class ServerHandshakeResponse : MessageBase
    {
        public string Name { get; set; }
        public override byte OpCode => 0x01;
    }
}