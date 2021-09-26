using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SerialDeviceSettingsViewModel : ViewModelBase, ISerialDeviceSettingsViewModel
    {
        public SerialDeviceSettingsViewModel(SerialDeviceSettings settings, SerialDeviceType serialDeviceType)
        {
            this.ResetCommand = new RelayCommand(_ => this.Reset());
            if (settings != null)
            {
                this.DriverName = settings.DriverName;
                this.SerialPort = settings.SerialPort;
                this.BaudRate = settings.BaudRate;
                this.Parity = settings.Parity;
                this.DataBits = settings.DataBits;
                this.StopBits = settings.StopBits;
            }
            this.IsReset = settings == null;
            this.SerialPortOptions = System.IO.Ports.SerialPort.GetPortNames();
            BaudRateOptions = new [] { 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
            ParityOptions = new []
            { 
                Models.Parity.None,
                Models.Parity.Odd,
                Models.Parity.Even,
                Models.Parity.Mark,
                Models.Parity.Space
            };
            DataBitOptions = new [] { 5, 6, 7, 8 };
            StopBitsOptions = new []
            {
                Models.StopBits.None,
                Models.StopBits.One,
                Models.StopBits.OnePointFive,
                Models.StopBits.Two,
            };
            if (serialDeviceType == SerialDeviceType.BarcodeScanner)
                DriverNames = new [] { "SerialBarcodeScanner" };
            else if (serialDeviceType == SerialDeviceType.Scale)
                DriverNames = new [] { "SerialScale" };
        }
        
        public string[] SerialPortOptions { get; }
        public int[] BaudRateOptions { get; }
        public Parity[] ParityOptions { get; }
        public int[] DataBitOptions { get; }
        public StopBits[] StopBitsOptions { get; }


        public bool IsReset { get; private set; }

        public bool IsValid => IsReset || (!string.IsNullOrEmpty(this.DriverName) && !string.IsNullOrEmpty(this.SerialPort) && this.BaudRate.HasValue && this.Parity.HasValue && this.DataBits.HasValue && this.StopBits.HasValue); 

        public ICommand ResetCommand { get; }
        
        public string DriverName { get; set; }

        public string[] DriverNames { get; }

        public string SerialPort { get; set; }
        
        public int? BaudRate { get; set; }
        
        public Parity? Parity { get; set; }
            
        public int? DataBits { get; set; }

        public StopBits? StopBits { get; set; }

        private void Reset()
        {
            this.DriverName = null;
            this.SerialPort = null;
            this.BaudRate = null;
            this.Parity = null;
            this.DataBits = null;
            this.StopBits = null;
            this.IsReset = true;
        }

        private void OnDriverNameChanged()
        {
            this.IsReset = false;
        }

        private void OnSerialPortChanged()
        {
            this.IsReset = false;
        }

        private void OnBaudRateChanged()
        {
            this.IsReset = false;
        }

        private void OnParityChanged()
        {
            this.IsReset = false;
        }

        private void OnDataBitsChanged()
        {
            this.IsReset = false;
        }

        private void OnStopBitsChanged()
        {
            this.IsReset = false;
        }
    }
}