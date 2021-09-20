namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ICultureBookInViewModel : IViewModel
    {
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
    }
}