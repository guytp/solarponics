using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ILoggedInView : IView
    {
        ILoggedInViewModel LoggedInViewModel { get; }
    }
}