using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IFruitingBlockIncubateShelfPlaceView : IView
    {
        IFruitingBlockIncubateShelfPlaceViewModel FruitingBlockIncubateShelfPlaceViewModel { get; }
    }
}