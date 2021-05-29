using Solarponics.Networking;
using Solarponics.Networking.Abstractions;

namespace Solarponics.IngestionServer.Net
{
    public class IngestionNetworkServer : NetworkServerBase
    {
        public IngestionNetworkServer(INetworkSessionFactory sessionFactory) : base(sessionFactory, 4201)
        {
        }
    }
}