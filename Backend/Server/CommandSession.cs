using System;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using NetCoreServer;
using Solarponics.Server.Handlers;
using Solarponics.Models.Messages;

namespace Solarponics.Server
{
    public class CommandSession: TcpSession
    {
        private readonly MessageHandler _messageHandler;
        public ClientHandshakeRequest ClientHandshake { get; set; }

        public CommandSession(TcpServer server) : base(server)
        {
            _messageHandler = new MessageHandler();
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"Command TCP session with Id {Id} connected!");
        }

        private void SendMessage(MessageBase message, bool async = true)
        {
            var raw = JsonSerializer.SerializeToUtf8Bytes(message, message.GetType());
            var output = new byte[raw.Length + 1];
            Array.Copy(raw, 0, output, 1, raw.Length);
            output[0] = message.OpCode;
            if (async)
                SendAsync(output);
            else
                Send(output);
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Command TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            MessageBase message = null;
            try
            {
                Console.WriteLine("Received " + size + " bytes at offset " + offset);
                var type = OpCodeToTypeConverter.TypeForOpCode(buffer[offset]);
                message =
                    (MessageBase) JsonSerializer.Deserialize(
                        buffer.Skip(1 + (int) offset).Take((int) size - 1).ToArray(), type);
                if (message == null)
                {
                    Disconnect();
                    return;
                }

                Console.WriteLine($"Incoming {message.GetType().Name}");

                var response = _messageHandler.HandleMessage(message, this);
                if (response == null)
                {
                    return;
                }

                SendMessage(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in session {Id} - disconnecting: {ex.Message}");
                SendMessage(new ErrorMessage("Unexpected error handling request: " + ex.Message, message?.Sequence ?? 0), false);
                while (this.BytesPending > 0)
                    Thread.Yield();
                Disconnect();
            }
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Command TCP session caught an error with code {error}");
        }
    }

}
