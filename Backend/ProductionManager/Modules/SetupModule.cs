using Solarponics.ProductionManager.Abstractions.Modules;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Modules
{
    public class SetupModule : ISetupModule
    {
        public SetupModule(ISetupSupplierView supplierView, ISetupRecipeView recipeView, ISetupHardwareView hardwareView)
        {
            this.MenuItems = new MenuItem[]
            {
                new MenuItem("Suppliers", supplierView),
                new MenuItem("Recipes", recipeView),
                new MenuItem("Hardware", hardwareView),
            } ;
        }
        public string Category => "Setup";

        public MenuItem[] MenuItems { get; }

        public int Order => 10000000;
    }
}