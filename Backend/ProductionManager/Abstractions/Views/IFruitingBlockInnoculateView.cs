using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IFruitingBlockInnoculateView : IView
    {
        IFruitingBlockInnoculateViewModel FruitingBlockInnoculateViewModel { get; }
    }
}