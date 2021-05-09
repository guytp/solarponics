namespace Solarponics.Models.Messages
{
    public class AcknowledgeResponse : MessageBase
    {
        public AcknowledgeResponse()
        {
        }

        public AcknowledgeResponse(ulong originalSequence)
        {
            OriginalSequence = originalSequence;
        }

        public ulong OriginalSequence { get; set; }
        public override byte OpCode => 0x02;
    }
}