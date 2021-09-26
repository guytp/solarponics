using Solarponics.ProductionManager.Data;
using System;

namespace Solarponics.ProductionManager.Abstractions.Hardware
{
    public interface IScale : IHardwareDevice
    {
        event EventHandler<WeightReadEventArgs> WeightRead;
    }
}