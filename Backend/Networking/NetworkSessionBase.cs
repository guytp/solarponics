using System;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using NetCoreServer;
using Solarponics.Models.Messages;
using Solarponics.Networking.Abstractions;
using Solarponics.Networking.Exceptions;

namespace Solarponics.Networking
{
    public abstract class NetworkSessionBase : TcpSession, INetworkSession
    {
        private readonly IMessageHandlerSelector _messageHandlerSelector;
        private readonly IOpCodeToTypeConverter _opCodeToTypeConverter;

        protected NetworkSessionBase(INetworkServer server, IMessageHandlerSelector messageHandlerSelector,
            IOpCodeToTypeConverter opCodeToTypeConverter) : base(server as TcpServer)
        {
            _messageHandlerSelector = messageHandlerSelector;
            _opCodeToTypeConverter = opCodeToTypeConverter;
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"TCP session with Id {Id} connected!");
        }

        private void SendMessage(IMessage message, bool async = true)
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
            Console.WriteLine($"TCP session with Id {Id} disconnected!");
        }

        protected override async void OnReceived(byte[] buffer, long offset, long size)
        {
            IMessage message = null;
            try
            {
                Console.WriteLine("Received " + size + " bytes at offset " + offset);
                var type = _opCodeToTypeConverter.TypeForOpCode(buffer[offset]);
                message =
                    (IMessage) JsonSerializer.Deserialize(
                        buffer.Skip(1 + (int) offset).Take((int) size - 1).ToArray(), type);
                if (message == null)
                {
                    Disconnect();
                    return;
                }

                Console.WriteLine($"Incoming {message.GetType().Name}");

                var handler = _messageHandlerSelector.GetHandlerForMessage(message);
                if (handler == null)
                    throw new NotImplementedException();

                var response = await handler.Handle(message, this);
                if (response == null) return;

                SendMessage(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in session {Id} - disconnecting: {ex.Message}");
                SendMessage(
                    new ErrorMessage("Unexpected error handling request: " + ex.Message, message?.Sequence ?? 0, (ex as ClientException)?.Urn),
                    false);
                while (BytesPending > 0)
                    Thread.Yield();
                Disconnect();
            }
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"TCP session caught an error with code {error}");
        }
    }
}