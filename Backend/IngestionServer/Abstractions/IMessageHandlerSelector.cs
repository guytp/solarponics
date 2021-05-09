using Solarponics.Models.Messages;

namespace Solarponics.IngestionServer.Abstractions
{
    public interface IMessageHandlerSelector
    {
        IMessageHandler GetHandlerForMessage(IMessage message);
    }
}