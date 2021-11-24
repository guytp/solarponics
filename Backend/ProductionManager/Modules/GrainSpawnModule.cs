using Solarponics.ProductionManager.Abstractions.Modules;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Modules
{
    public class GrainSpawnModule : IGrainSpawnModule
    {
        public GrainSpawnModule(IGrainSpawnCreateView createView, IGrainSpawnInnoculateView innoculateView, IGrainSpawnShelfPlaceView shelfPlaceView, IGrainSpawnListView listView)
        {
            this.MenuItems = new [] { new MenuItem("Create", createView), new MenuItem("Shelf Place", shelfPlaceView), new MenuItem("Innoculate", innoculateView), new MenuItem("List", listView) } ;
        }
        public string Category => "Grain Spawn";

        public MenuItem[] MenuItems { get; }

        public int Order => 5000000;
    }
}