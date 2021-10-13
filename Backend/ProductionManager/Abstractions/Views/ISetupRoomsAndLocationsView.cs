using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ISetupRoomsAndLocationsView : IView
    {
        ISetupRoomsAndLocationsViewModel SetupRoomsAndLocationsViewModel { get; }
    }
}