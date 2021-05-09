namespace Solarponics.Models.Messages
{
    public class ErrorMessage : MessageBase
    {
        public ErrorMessage()
        {
        }

        public ErrorMessage(string message, ulong originalSequence)
        {
            Message = message;
            OriginalSequence = originalSequence;
        }

        public ulong OriginalSequence { get; set; }
        public string Message { get; set; }
        public override byte OpCode => 0x05;
    }
}