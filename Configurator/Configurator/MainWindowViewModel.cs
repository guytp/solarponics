using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Windows.Input;

namespace Configurator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            SetupCommand = new RelayCommand(_ => Setup());
            Ssid = "YoghurtFancier";
            SsidKey = "AABBCC0011";
            IpAddress = "10.2.0.88";
            Broadcast = "255.255.255.0";
            Gateway = "10.2.0.1";
            DnsServer = "10.2.0.5";
            Server = "10.2.0.100";
            UniqueIdentifier = Guid.NewGuid().ToString();
            Name = "SP-CFS-SNS-01";
        }

        public string Log { get; private set; }

        public ICommand SetupCommand { get; }

        public string UniqueIdentifier { get; set; }
        public string Name { get; set; }

        public string Ssid { get; set; }
        public string SsidKey { get; set; }
        public string IpAddress { get; set; }
        public string Broadcast { get; set; }
        public string Gateway { get; set; }
        public string DnsServer { get; set; }
        public string Server { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void Setup()
        {
            AddLog("Setup() connecting to 192.168.4.1");

            try
            {
                using var client = new TcpClient("192.168.4.1", 5900) {ReceiveTimeout = 1000, SendTimeout = 1000};
                var data = Encoding.ASCII.GetBytes("si=" + Ssid + "&sk=" + SsidKey + "&ip=" + IpAddress + "&bc=" +
                                                   Broadcast + "&gw=" + Gateway + "&d1=" + DnsServer + "&sr=" + Server + "&ui=" + UniqueIdentifier + "&nm=" + Name);
                var dataLen = (byte) data.Length;
                using var stream = client.GetStream();
                stream.Write(new[] {dataLen});
                stream.Write(data);
                var bytes = stream.Read(data, 0, data.Length);
                var responseData = Encoding.ASCII.GetString(data, 0, bytes);
                var responseParts = responseData.Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries);
                var res = "Unknown";
                var serial = "Unknown";
                foreach (var responsePart in responseParts)
                {
                    var keyValueParts = responsePart.Split(new[] {'='});
                    var val = keyValueParts[1];
                    switch (keyValueParts[0])
                    {
                        case "res":
                            res = val;
                            break;
                        case "ser":
                            serial = val;
                            break;
                    }
                }

                AddLog("Result: " + res);
                AddLog("Serial: " + serial);
                AddLog(responseData);
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                AddLog("Setup encountered error " + ex);
                return;
            }

            AddLog("Setup complete");
        }

        private void AddLog(string text)
        {
            Log = text + Environment.NewLine + Log;
        }
    }
}