using Solarponics.Networking;
using Solarponics.Networking.Abstractions;

namespace Solarponics.ProvisioningServer.Net
{
    public class ProvisioningNetworkServer : NetworkServerBase
    {
        public ProvisioningNetworkServer(INetworkSessionFactory sessionFactory) : base(sessionFactory, 4202)
        {
        }
    }
}