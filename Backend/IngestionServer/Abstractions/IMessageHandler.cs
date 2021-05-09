using Solarponics.Models.Messages;

namespace Solarponics.IngestionServer.Abstractions
{
    public interface IMessageHandler
    {
        IMessage Handle(IMessage inbound, INetworkSession session);
    }
}