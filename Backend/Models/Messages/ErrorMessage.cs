namespace Solarponics.Models.Messages
{
    public class ErrorMessage : MessageBase
    {
        public ErrorMessage()
        {
        }

        public ErrorMessage(string message, ulong originalSequence, string urn)
        {
            Message = message;
            OriginalSequence = originalSequence;
            Urn = urn;
        }

        public ulong OriginalSequence { get; set; }
        public string Message { get; set; }
        public string Urn { get; set; }
        public override byte OpCode => 0x05;
    }
}