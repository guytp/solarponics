using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ISetupAutoclavesView : IView
    {
        ISetupAutoclavesViewModel SetupAutoclavesViewModel { get; }
    }
}