using System.Windows.Input;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;

namespace Solarponics.ProductionManager.ViewModels
{
    public class LoggedInViewModel : ViewModelBase, ILoggedInViewModel
    {
        private readonly IAuthenticationSession authenticationSession;
        private readonly INavigator navigator;

        public LoggedInViewModel(INavigator navigator, IAuthenticationSession authenticationSession)
        {
            this.navigator = navigator;
            this.authenticationSession = authenticationSession;
            LogoutCommand = new RelayCommand(_ => Logout());
        }

        public User User { get; set; }
        public ICommand LogoutCommand { get; }

        private void Logout()
        {
            User = null;
            authenticationSession.SetUser(null);
            navigator.ReturnToLogin();
        }
    }
}