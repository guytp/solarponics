using Serilog;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Solarponics.ProductionManager.Hardware
{
    public abstract class SerialDeviceBase : IDisposable
    {
        private readonly SerialDeviceSettings settings;
        private SerialPort serialPort;
        private readonly byte[] dataSuffix;
        private readonly List<byte> bufferCarryOver = new List<byte>(10240);

        public SerialDeviceBase(SerialDeviceSettings settings, byte[] dataSuffix)
        {
            this.settings = settings;
            this.dataSuffix = dataSuffix;
        }

        public void Start()
        {
            // Give ourselves up to five tries to open the port
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    // Always clean up first
                    this.Dispose();

                    var stopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), settings.StopBits.ToString());
                    var parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), settings.Parity.ToString());
                    this.serialPort = new SerialPort(settings.SerialPort, settings.BaudRate, parity, settings.DataBits, stopBits)
                    {
                        ReadTimeout = 500
                    };
                    this.serialPort.Open();
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"Error opening serial port on attempt {i + 1}");
                    if (i == 4)
                        throw;

                    Thread.Sleep(200);
                }
            }

            this.serialPort.DiscardInBuffer();
            this.serialPort.DiscardOutBuffer();
            while (this.serialPort.BytesToRead > 0)
            {
                try
                {
                    var discardBuffer = new byte[this.serialPort.BytesToRead];
                    this.serialPort.Read(discardBuffer, 0, discardBuffer.Length);
                }
                catch
                {
                    // Intentionally swallowed
                    break;
                }
            }
            this.serialPort.DataReceived += OnDataReceived;
        }

        public void Dispose()
        {
            try
            {
                this.serialPort?.Close();
            }
            catch
            {
                // Intentionally swallowed
            }
            try
            {
                this.serialPort?.Dispose();
            }
            catch
            {
                // Intentionally swallowed
            }

            this.serialPort = null;
        }
        

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var read = this.serialPort.BytesToRead;
                var buffer = new byte[read];
                read = this.serialPort.Read(buffer, 0, read);

                if (this.bufferCarryOver.Count > 0)
                {
                    for (int i = 0; i < read; i++)
                        this.bufferCarryOver.Add(buffer[i]);
                    buffer = this.bufferCarryOver.ToArray();
                    this.bufferCarryOver.Clear();
                    read = buffer.Length;
                }
                
                ParseBuffer(buffer, read);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal error in " + GetType().Name + ".  This device will be turned off." + Environment.NewLine + Environment.NewLine + ex);
                this.Dispose();
                return;
            }
        }

        private void ParseBuffer(byte[] buffer, int length)
        {
            if (this.dataSuffix == null || this.dataSuffix.Length == 0)
            {
                var str = System.Text.Encoding.ASCII.GetString(buffer);
                if (!string.IsNullOrWhiteSpace(str))
                    StringRead(str);
                return;
            }

            var suffixCheckBuffer = new byte[this.dataSuffix.Length];

            var startFrom = 0;
            for (var i = 0; i < length; i++)
            {
                if (i < this.dataSuffix.Length)
                    suffixCheckBuffer[i] = buffer[i];
                else
                {
                    for (var j = 0; j < suffixCheckBuffer.Length - 1; j++)
                        suffixCheckBuffer[j] = suffixCheckBuffer[j + 1];
                    suffixCheckBuffer[^1] = buffer[i];
                }

                if (suffixCheckBuffer.SequenceEqual(this.dataSuffix))
                {
                    var str = System.Text.Encoding.ASCII.GetString(buffer, startFrom, i - this.dataSuffix.Length + 1);
                    if (!string.IsNullOrWhiteSpace(str))
                        StringRead(str);
                    startFrom = i;
                    for (var j = 0; j < this.dataSuffix.Length; j++)
                        suffixCheckBuffer[j] = 0;
                }
            }

            if (startFrom != length - 1)
                for (var i = startFrom; i < length; i++)
                    this.bufferCarryOver.Add(buffer[i]);
        }

       protected abstract void StringRead(string str);
    }
}