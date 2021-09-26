using Solarponics.Models;

namespace Solarponics.ProductionManager.Abstractions.Hardware
{
    public interface IDriverProvider
    {
        THardwareDevice Get<THardwareDevice>(IDriverSettings settings) where THardwareDevice:IHardwareDevice;
    }
}