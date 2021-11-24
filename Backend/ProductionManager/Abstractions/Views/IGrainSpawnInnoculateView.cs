using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IGrainSpawnInnoculateView : IView
    {
        IGrainSpawnInnoculateViewModel GrainSpawnInnoculateViewModel { get; }
    }
}