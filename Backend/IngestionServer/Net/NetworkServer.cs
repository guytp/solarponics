using System;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;
using Solarponics.IngestionServer.Abstractions;

namespace Solarponics.IngestionServer.Net
{
    public class NetworkServer : TcpServer, INetworkServer
    {
        private readonly INetworkSessionFactory _sessionFactory;

        public NetworkServer(IPAddress address, int port, INetworkSessionFactory sessionFactory) : base(address, port)
        {
            _sessionFactory = sessionFactory;
        }

        protected override TcpSession CreateSession()
        {
            return (TcpSession) _sessionFactory.Create(this);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Command TCP server caught an error with code {error}");
        }
    }
}