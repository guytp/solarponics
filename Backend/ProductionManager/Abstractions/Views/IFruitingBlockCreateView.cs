using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IFruitingBlockCreateView : IView
    {
        IFruitingBlockCreateViewModel FruitingBlockCreateViewModel { get; }
    }
}