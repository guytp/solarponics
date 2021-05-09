using System;
using System.Net.Sockets;
using System.Text;
using Solarponics.SensorModuleEmulator.EventArgs;
using TcpClient = NetCoreServer.TcpClient;

namespace Solarponics.SensorModuleEmulator.Net
{
    public class CommandClient : TcpClient
    {
        public CommandClient(string address, int port) : base(address, port)
        {
        }

        public event EventHandler<LogEventArgs> LogMessage;

        protected override void OnConnected()
        {
            Log($"TCP client connected a new session with Id {Id}");
        }

        protected override void OnDisconnected()
        {
            Log($"TCP client disconnected a session with Id {Id}");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            Log(Encoding.UTF8.GetString(buffer, (int) offset + 1, (int) size - 1));
        }

        protected override void OnError(SocketError error)
        {
            Log($"Chat TCP client caught an error with code {error}");
        }

        private void Log(string log)
        {
            LogMessage?.Invoke(this, new LogEventArgs(log));
        }
    }
}