using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IModuleContainer
    {
        string[] Categories { get; }

        MenuItem[] GetMenuItemsForCategory(string category);
    }
}