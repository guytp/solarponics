using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ISetupShelvesView : IView
    {
        ISetupShelvesViewModel SetupShelvesViewModel { get; }
    }
}