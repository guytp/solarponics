namespace Solarponics.Models.Messages.Ingestion
{
    public class TimeRequest : MessageBase
    {
        public override byte OpCode => 0x06;
    }
}