using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IEnvironmentSensorReadingsAggregateView : IView
    {
        IEnvironmentSensorReadingsAggregateViewModel EnvironmentSensorReadingsAggregateViewModel { get; }
    }
}