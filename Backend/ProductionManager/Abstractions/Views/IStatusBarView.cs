using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IStatusBarView
    {
        IStatusBarViewModel StatusBarViewModel { get; }
    }
}