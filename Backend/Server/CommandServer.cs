using System;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace Solarponics.Server
{
    public class CommandServer : TcpServer
    {
        public CommandServer(IPAddress address, int port) : base(address, port) {}

        protected override TcpSession CreateSession() { return new CommandSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Command TCP server caught an error with code {error}");
        }
    }
}
