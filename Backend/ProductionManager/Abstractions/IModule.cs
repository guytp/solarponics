using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IModule
    {
        string Category { get; }

        MenuItem[] MenuItems { get; }

        int Order { get; }
    }
}