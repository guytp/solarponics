using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Core;

namespace Solarponics.ProductionManager.ViewModels
{
    public class CultureBookInViewModel : ViewModelBase, ICultureBookInViewModel
    {
        public CultureBookInViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
    }
}