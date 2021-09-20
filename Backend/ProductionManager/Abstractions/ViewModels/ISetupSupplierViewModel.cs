namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupSupplierViewModel : IViewModel
    {
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
    }
}