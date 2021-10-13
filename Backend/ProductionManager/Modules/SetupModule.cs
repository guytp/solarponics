using Solarponics.ProductionManager.Abstractions.Modules;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Modules
{
    public class SetupModule : ISetupModule
    {
        public SetupModule(ISetupSupplierView supplierView, ISetupRecipeView recipeView, ISetupHardwareView hardwareView, ISetupRoomsAndLocationsView roomsAndLocationsView)
        {
            this.MenuItems = new MenuItem[]
            {
                new MenuItem("Hardware", hardwareView),
                new MenuItem("Suppliers", supplierView),
                new MenuItem("Recipes", recipeView),
                new MenuItem("Rooms", roomsAndLocationsView)
            };
        }
        public string Category => "Setup";

        public MenuItem[] MenuItems { get; }

        public int Order => 10000000;
    }
}