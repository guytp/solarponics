using System;
using System.Net;
using System.Net.Sockets;

namespace Solarponics.ModbusIngestionProxy
{
    public interface ICommandClient
    {
        event EventHandler Connected;
        event EventHandler Disconnected;
        event EventHandler Received;
        Guid Id { get; }
        IPEndPoint Endpoint { get; }
        Socket Socket { get; }
        long BytesPending { get; }
        long BytesSending { get; }
        long BytesSent { get; }
        bool OptionDualMode { get; set; }
        bool OptionKeepAlive { get; set; }
        bool OptionNoDelay { get; set; }
        int OptionReceiveBufferSize { get; set; }
        int OptionSendBufferSize { get; set; }
        bool IsConnecting { get; }
        bool IsConnected { get; }
        long BytesReceived { get; }
        bool IsDisposed { get; }
        bool IsSocketDisposed { get; }

        bool Connect();
        bool ConnectAsync();
        bool Disconnect();
        bool DisconnectAsync();
        void Dispose();
        long Receive(byte[] buffer, long offset, long size);
        long Receive(byte[] buffer);
        string Receive(long size);
        void ReceiveAsync();
        bool Reconnect();
        bool ReconnectAsync();
        long Send(string text);
        long Send(byte[] buffer, long offset, long size);
        long Send(byte[] buffer);
        bool SendAsync(string text);
        bool SendAsync(byte[] buffer, long offset, long size);
        bool SendAsync(byte[] buffer);
    }
}