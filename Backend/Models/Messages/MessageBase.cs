namespace Solarponics.Models.Messages
{
    public abstract class MessageBase
    {
        private static ulong _sequence;
        private static readonly object SequenceLocker = new object();

        protected MessageBase()
        {
            lock (SequenceLocker)
            {
                Sequence = _sequence++;
            }
        }

        public abstract byte OpCode { get; }

        public ulong Sequence { get; set; }
    }
}