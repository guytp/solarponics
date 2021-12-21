using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IFruitingBlockFruitingShelfPlaceView : IView
    {
        IFruitingBlockFruitingShelfPlaceViewModel FruitingBlockFruitingShelfPlaceViewModel { get; }
    }
}