using Solarponics.ProductionManager.Data;
using System;

namespace Solarponics.ProductionManager.Abstractions.Hardware
{
    public interface IBarcodeScanner : IHardwareDevice
    {
        event EventHandler<BarcodeReadEventArgs> BarcodeRead;
    }
}