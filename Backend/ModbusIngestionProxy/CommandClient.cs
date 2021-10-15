using System;
using System.Net.Sockets;
using System.Text;
using TcpClient = NetCoreServer.TcpClient;

namespace Solarponics.ModbusIngestionProxy
{
    public class CommandClient : TcpClient, ICommandClient
    {
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler Received;
        public CommandClient(string address, int port) : base(address, port)
        {
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"TCP client connected a new session with Id {Id}");
            Connected?.Invoke(this, new EventArgs());
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"TCP client disconnected a session with Id {Id}");
            Disconnected?.Invoke(this, new EventArgs());
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            Console.WriteLine("<< " + Encoding.UTF8.GetString(buffer, (int) offset + 1, (int) size - 1));
            Received?.Invoke(this, new EventArgs());
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP client caught an error with code {error}");
        }
    }
}