using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IGrainSpawnMixView : IView
    {
        IGrainSpawnMixViewModel GrainSpawnMixViewModel { get; }
    }
}