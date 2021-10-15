using System;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using Solarponics.Models;
using Solarponics.Models.Messages;
using Solarponics.Models.Messages.Ingestion;
using Solarponics.SensorModuleEmulator.Net;

namespace Solarponics.SensorModuleEmulator
{
    public class SensorModuleEmulatorViewModel : INotifyPropertyChanged
    {
        private readonly CommandClient _client;
        private ulong _sequence;

        public SensorModuleEmulatorViewModel()
        {
            ConnectCommand = new RelayCommand(_ => Connect());
            DisconnectCommand = new RelayCommand(_ => Disconnect());
            HandshakeCommand = new RelayCommand(_ => Handshake());
            TemperatureReadingCommand =
                new RelayCommand(_ => SendReading(SensorType.Temperature, decimal.Parse(Temperature)));
            HumidityReadingCommand =
                new RelayCommand(_ => SendReading(SensorType.Humidity, decimal.Parse(Humidity)));
            CarbonDioxideReadingCommand =
                new RelayCommand(_ => SendReading(SensorType.CarbonDioxide, decimal.Parse(CarbonDioxide)));
            Temperature = "15.2";
            Humidity = "57.3";
            SerialNumber = "SN01";
            CarbonDioxide = "1532";
            _client = new CommandClient("127.0.0.1", 4201);
            _client.LogMessage += (_, e) => AddLog(e.Log);
        }

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand HandshakeCommand { get; }
        public ICommand TemperatureReadingCommand { get; }
        public ICommand HumidityReadingCommand { get; }
        public ICommand CarbonDioxideReadingCommand { get; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string CarbonDioxide { get; set; }
        public string SerialNumber { get; set; }
        public string Log { get; private set; }

#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

        private void Connect()
        {
            _client.Connect();
        }

        private void Disconnect()
        {
            _client.Disconnect();
        }

        private void Handshake()
        {
            SendCommand(new ClientHandshakeRequest
            {
                Sequence = _sequence++,
                SerialNumber = SerialNumber,
            });
        }

        private void SendReading(SensorType type, decimal reading)
        {
            SendCommand(new NewSensorReading
            {
                Sequence = _sequence++,
                Type = type,
                Reading = reading,
                Number = 1,
                Timestamp = (ulong)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds
            });
        }

        private void SendCommand(MessageBase message)
        {
            var raw = JsonSerializer.SerializeToUtf8Bytes(message, message.GetType());
            var output = new byte[raw.Length + 1];
            Array.Copy(raw, 0, output, 1, raw.Length);
            output[0] = message.OpCode;
            _client.SendAsync(output);

            AddLog("Sent " + output.Length + " bytes for OpCode " + message.OpCode);
            this._client.ReceiveAsync();
        }

        private void AddLog(string message)
        {
            Log = message + Environment.NewLine + Log;
        }
    }
}