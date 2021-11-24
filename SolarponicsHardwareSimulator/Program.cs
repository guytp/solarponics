using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace PhotoscribeSerialSimulator
{
    class Program
    {
        SerialPort barcodeScannerPort;
        SerialPort scalePort;

        static void Main(string[] args)
        {
            var p  = new Program();
            p.Start();
            while (true)
            {
                Console.Write("Type [B/S]: ");
                var key = Console.ReadKey().Key;
                Console.WriteLine();
                if (key != ConsoleKey.B && key != ConsoleKey.S)
                {
                    continue;
                }

                if (key == ConsoleKey.B)
                    Console.Write("Barcode: ");
                else
                    Console.Write("Weight: ");
                var line = Console.ReadLine();

                if (key == ConsoleKey.S && !decimal.TryParse(line, out _))
                {
                    Console.WriteLine("Invalid decimal");
                    continue;
                }
                p.Send(line, key == ConsoleKey.B ? p.barcodeScannerPort : p.scalePort);
            }
        }

        void Start()
        {
            this.barcodeScannerPort = new SerialPort("COM2", 9600, Parity.None)
            {
                DataBits = 8,
                Handshake = Handshake.None,
                ReadTimeout = 500,
                StopBits = StopBits.One
            };
            this.barcodeScannerPort.Open();

            this.scalePort = new SerialPort("COM4", 9600, Parity.None)
            {
                DataBits = 8,
                Handshake = Handshake.None,
                ReadTimeout = 500,
                StopBits = StopBits.One
            };
            this.scalePort.Open();
        }

        private void Send(string buf, SerialPort port)
        {
            //Console.WriteLine("Out: " + buf);
            var bytes = System.Text.Encoding.ASCII.GetBytes(buf);
            var all = new List<byte>();
            all.AddRange(bytes);
            all.Add((byte)'\r');
            var outBuf = all.ToArray();
            port.Write(outBuf, 0, outBuf.Length);
        }
    }
}