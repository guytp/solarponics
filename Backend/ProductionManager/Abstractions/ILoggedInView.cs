using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface ILoggedInView : IView
    {
        ILoggedInViewModel LoggedInViewModel { get; }
    }
}