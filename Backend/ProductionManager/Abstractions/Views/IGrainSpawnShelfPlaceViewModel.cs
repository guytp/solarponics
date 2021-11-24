using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IGrainSpawnShelfPlaceView : IView
    {
        IGrainSpawnShelfPlaceViewModel GrainSpawnShelfPlaceViewModel { get; }
    }
}