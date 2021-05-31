namespace Solarponics.Models.Messages.Ingestion
{
    public class TimeResponse : MessageBase
    {
        public ulong Timestamp { get; set; }
        public override byte OpCode => 0x07;
    }
}