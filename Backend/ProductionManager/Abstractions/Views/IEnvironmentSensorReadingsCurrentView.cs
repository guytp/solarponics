using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IEnvironmentSensorReadingsCurrentView : IView
    {
        IEnvironmentSensorReadingsCurrentViewModel EnvironmentSensorReadingsCurrentViewModel { get; }
    }
}