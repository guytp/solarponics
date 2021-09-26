using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Abstractions.Hardware
{
    public interface ILabelPrinter : IHardwareDevice
    {
        void Print(LabelDefinition label);
    }
}