using System;
using System.Collections.Generic;
using System.Text;

namespace Solarponics.IngestionServer.Abstractions
{
    public interface INetworkSessionFactory
    {
        INetworkSession Create(INetworkServer server);
    }
}