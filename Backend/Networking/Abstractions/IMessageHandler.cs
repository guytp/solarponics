using System.Threading.Tasks;
using Solarponics.Models.Messages;

namespace Solarponics.Networking.Abstractions
{
    public interface IMessageHandler
    {
        Task<IMessage> Handle(IMessage inbound, INetworkSession session);
    }
}