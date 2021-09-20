using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ISetupHardwareView : IView
    {
        ISetupHardwareViewModel SetupHardwareViewModel { get; }
    }
}