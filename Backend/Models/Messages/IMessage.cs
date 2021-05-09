namespace Solarponics.Models.Messages
{
    public interface IMessage
    {
        byte OpCode { get; }

        ulong Sequence { get; set; }
    }
}