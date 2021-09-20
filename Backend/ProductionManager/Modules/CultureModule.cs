using Solarponics.ProductionManager.Abstractions.Modules;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Modules
{
    public class CultureModule : ICultureModule
    {
        public CultureModule(ICultureBookInView bookInView)
        {
            this.MenuItems = new [] { new MenuItem("Book-In", bookInView) } ;
        }
        public string Category => "Culture";

        public MenuItem[] MenuItems { get; }

        public int Order => 5000000;
    }
}