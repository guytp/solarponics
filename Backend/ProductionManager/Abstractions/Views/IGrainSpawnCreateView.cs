using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IGrainSpawnCreateView : IView
    {
        IGrainSpawnCreateViewModel GrainSpawnCreateViewModel { get; }
    }
}