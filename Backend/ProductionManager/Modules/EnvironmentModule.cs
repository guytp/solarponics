using Solarponics.ProductionManager.Abstractions.Modules;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Modules
{
    public class EnvironmentModule : IEnvironmentModule
    {
        public EnvironmentModule(IEnvironmentSensorReadingsCurrentView environmentModuleSensorReadingsCurrentView, IEnvironmentSensorReadingsAggregateView environmentModuleSensorReadingsAggregateView)
        {
            this.MenuItems = new [] { new MenuItem("Current Readings", environmentModuleSensorReadingsCurrentView), new MenuItem("Historic Readings", environmentModuleSensorReadingsAggregateView) } ;
        }
        public string Category => "Environment";

        public MenuItem[] MenuItems { get; }

        public int Order => 5000000;
    }
}