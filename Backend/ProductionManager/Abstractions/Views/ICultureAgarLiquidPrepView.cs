using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ICultureAgarLiquidPrepView : IView
    {
        ICultureAgarLiquidPrepViewModel CultureAgarLiquidPrepViewModel { get; }
    }
}