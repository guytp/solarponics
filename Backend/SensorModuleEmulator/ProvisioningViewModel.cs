using System;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using Solarponics.Models.Messages;
using Solarponics.Models.Messages.Provisioning;
using Solarponics.SensorModuleEmulator.Net;

namespace Solarponics.SensorModuleEmulator
{
    public class ProvisioningViewModel : INotifyPropertyChanged
    {
        private readonly CommandClient _client;
        private ulong _sequence;

        public ProvisioningViewModel()
        {
            ConnectCommand = new RelayCommand(_ => Connect());
            DisconnectCommand = new RelayCommand(_ => Disconnect());
            ProvisionCommand = new RelayCommand(_ => Provision());
            SerialNumber = "SN03";
            _client = new CommandClient("127.0.0.1", 4203);
            _client.LogMessage += (_, e) => AddLog(e.Log);
        }

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand ProvisionCommand { get; }
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

        private void Provision()
        {
            SendCommand(new ProvisioningRequest
            {
                Sequence = _sequence++,
                SerialNumber = this.SerialNumber,
            });
        }

        private void SendCommand(IMessage message)
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