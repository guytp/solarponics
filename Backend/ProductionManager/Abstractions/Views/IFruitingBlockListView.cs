using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IFruitingBlockListView : IView
    {
        IFruitingBlockListViewModel FruitingBlockListViewModel { get; }
    }
}