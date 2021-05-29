using Solarponics.Models.Messages;

namespace Solarponics.Networking.Abstractions
{
    public interface IMessageHandlerSelector
    {
        IMessageHandler GetHandlerForMessage(IMessage message);
    }
}