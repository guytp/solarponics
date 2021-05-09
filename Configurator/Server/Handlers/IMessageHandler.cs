using Solarponics.Models.Messages;

namespace Solarponics.Server.Handlers
{
    public interface IMessageHandler
    {
        MessageBase Handle(MessageBase inbound, CommandSession session);
    }
}