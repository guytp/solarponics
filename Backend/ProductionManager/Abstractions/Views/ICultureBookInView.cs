using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ICultureBookInView : IView
    {
        ICultureBookInViewModel CultureBookInViewModel { get; }
    }
}