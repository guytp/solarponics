using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Core;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupSupplierViewModel : ViewModelBase, ISetupSupplierViewModel
    {
        public SetupSupplierViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
    }
}