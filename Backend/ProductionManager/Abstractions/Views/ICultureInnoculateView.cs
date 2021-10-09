using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ICultureInnoculateView : IView
    {
        ICultureInnoculateViewModel CultureInnoculateViewModel { get; }
    }
}