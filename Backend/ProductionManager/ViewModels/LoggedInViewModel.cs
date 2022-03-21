using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.ViewModels
{
    public class LoggedInViewModel : ViewModelBase, ILoggedInViewModel
    {
        public LoggedInViewModel(IAuthenticationSession authenticationSession, IMainMenuItemCategoryViewModel mainMenuItemCategoryViewModel, ILoggedInButtonsViewModel loggedInButtonsViewModel)
        {
            authenticationSession.LoggedOut += OnLogout;
            this.MainMenuItemCategoryViewModel = mainMenuItemCategoryViewModel;
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
        }

        public User User { get; set; }
        
        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public IMainMenuItemCategoryViewModel MainMenuItemCategoryViewModel { get; }
        
        private void OnLogout(object sender, System.EventArgs e)
        {
            this.User = null;
        }
    }
}