using Solarponics.ProductionManager.Abstractions.Modules;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Modules
{
    public class FruitingBlockModule : IFruitingBlockModule
    {
        public FruitingBlockModule(IFruitingBlockCreateView createView, IFruitingBlockInnoculateView innoculateView, IFruitingBlockFruitingShelfPlaceView fruitingShelfPlaceView, IFruitingBlockIncubateShelfPlaceView incubateShelfPlaceView, IFruitingBlockListView listView)
        {
            this.MenuItems = new [] { new MenuItem("Create", createView), new MenuItem("Fruiting Shelf Place", fruitingShelfPlaceView), new MenuItem("Incubate Shelf Place", incubateShelfPlaceView), new MenuItem("Innoculate", innoculateView), new MenuItem("List", listView) } ;
        }
        public string Category => "Fruiting Block";

        public MenuItem[] MenuItems { get; }

        public int Order => 5000000;
    }
}