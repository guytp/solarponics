namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupRecipeViewModel : IViewModel
    {
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
    }
}