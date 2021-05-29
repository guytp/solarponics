using System;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;
using Solarponics.Networking.Abstractions;

namespace Solarponics.Networking
{
    public class NetworkServerBase : TcpServer, INetworkServer
    {
        private readonly INetworkSessionFactory _sessionFactory;

        public NetworkServerBase(INetworkSessionFactory sessionFactory, int port) : base(IPAddress.Any, port)
        {
            _sessionFactory = sessionFactory;
        }

        protected override TcpSession CreateSession()
        {
            return (TcpSession) _sessionFactory.Create(this);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"TCP server caught an error with code {error}");
        }
    }
}