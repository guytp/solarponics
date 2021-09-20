namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupHardwareViewModel : IViewModel
    {
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
    }
}