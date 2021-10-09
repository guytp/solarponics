using Solarponics.ProductionManager.Abstractions.Modules;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Modules
{
    public class CultureModule : ICultureModule
    {
        public CultureModule(ICultureBookInView bookInView, ICultureAgarLiquidPrepView liquidAgarPrepView)
        {
            this.MenuItems = new [] { new MenuItem("Book-In", bookInView), new MenuItem("Agar / Liquid Prep", liquidAgarPrepView) } ;
        }
        public string Category => "Culture";

        public MenuItem[] MenuItems { get; }

        public int Order => 5000000;
    }
}