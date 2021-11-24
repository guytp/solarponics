using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IGrainSpawnListView : IView
    {
        IGrainSpawnListViewModel GrainSpawnListViewModel { get; }
    }
}