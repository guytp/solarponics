namespace Solarponics.Models.Messages.Ingestion
{
    public class ClientHandshakeRequest : MessageBase
    {
        public string SerialNumber { get; set; }
        public override byte OpCode => 0x04;
    }
}