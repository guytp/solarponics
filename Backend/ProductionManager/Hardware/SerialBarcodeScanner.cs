using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Data;
using System;

namespace Solarponics.ProductionManager.Hardware
{
    public class SerialBarcodeScanner : SerialDeviceBase, IBarcodeScanner
    {
        public SerialBarcodeScanner(BarcodeScannerSettings settings)
            : base(settings, new byte[] { (byte)'\r' })
        {
        }

        public event EventHandler<BarcodeReadEventArgs> BarcodeRead;

        protected override void StringRead(string str)
        {
            var d = System.Windows.Application.Current?.Dispatcher;

            var act = new Action(() =>
            {
                this.BarcodeRead?.Invoke(this, new BarcodeReadEventArgs(str));
            });

            if (d == null || d.CheckAccess())
                act();
            else
                d.Invoke(act);
        }
    }
}