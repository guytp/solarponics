using Microsoft.Extensions.Configuration;
using Solarponics.Models;
using Solarponics.Models.Messages;
using Solarponics.Models.Messages.Ingestion;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Solarponics.ModbusIngestionProxy
{
    public class IngestionClient : IIngestionClient, IDisposable
    {
        private readonly ICommandClient client;
        private ulong sequence = 0;
        private bool isStarted;
        private string serialNumber;
        private readonly object locker = new object();
        private bool awaitingActionComplete;

        public IngestionClient(IConfiguration configuration, ICommandClientFactory factory)
        {
            var ipAddress = configuration["ingestionServer:ip"];
            var port = short.Parse(configuration["ingestionServer:port"]);
            this.client = factory.Create(ipAddress, port);
            this.client.Connected += OnConnected;
            this.client.Disconnected += OnDisconnected;
            this.client.Received += OnReceived;
        }

        private void OnReceived(object sender, EventArgs e)
        {
            awaitingActionComplete = true;
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            isStarted = false;
            awaitingActionComplete = true;
        }

        private void OnConnected(object sender, EventArgs e)
        {
            SendCommand(new ClientHandshakeRequest
            {
                Sequence = sequence++,
                SerialNumber = serialNumber,
            });
            isStarted = true;
            awaitingActionComplete = true;
        }

        public void Start(string serialNumber)
        {
            if (string.IsNullOrEmpty(serialNumber))
                throw new ArgumentNullException(nameof(serialNumber));
            this.serialNumber = serialNumber;
            awaitingActionComplete = false;
            this.client.Connect();
            while (!awaitingActionComplete)
                Thread.Sleep(100);
        }
        public bool IsConnectedAndStarted => this.client.IsConnected && isStarted;

        public void SendReading(SensorType type, byte number, decimal reading)
        {
            if (!IsConnectedAndStarted)
            {
                throw new Exception("Not connected to ingestion server");
            }
            SendCommand(new NewSensorReading
            {
                Sequence = sequence++,
                Type = type,
                Reading = reading,
                Number = number,
                Timestamp = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds
            });
        }

        public void Dispose()
        {
            if (this.client.IsConnected)
                this.client.Disconnect();
            this.client.Dispose();
            isStarted = false;
        }

        private void SendCommand(MessageBase message)
        {
            lock (locker)
            {
                var raw = JsonSerializer.SerializeToUtf8Bytes(message, message.GetType());
                var output = new byte[raw.Length + 1];
                Array.Copy(raw, 0, output, 1, raw.Length);
                output[0] = message.OpCode;
                client.SendAsync(output);
                Console.WriteLine(">> " + Encoding.UTF8.GetString(output, (int)1, (int)raw.Length));
                Console.WriteLine("Sent " + output.Length + " bytes for OpCode " + message.OpCode);
                awaitingActionComplete = false;
                this.client.ReceiveAsync();
                while (!awaitingActionComplete)
                    Thread.Sleep(100);
            }
        }
    }
}
