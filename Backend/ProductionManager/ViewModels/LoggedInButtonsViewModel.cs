using PropertyChanged;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class LoggedInButtonsViewModel : ViewModelBase, ILoggedInButtonsViewModel
    {
        private readonly IAuthenticationSession authenticationSession;
        private readonly INavigator navigator;

        public LoggedInButtonsViewModel(IAuthenticationSession authenticationSession, INavigator navigator)
        {
            this.navigator = navigator;
            this.navigator.ViewChanged += OnViewChanged;
            this.authenticationSession = authenticationSession;
            LogoutCommand = new RelayCommand(_ => Logout());
            HomeCommand = new RelayCommand(_ => Home());
            ExitCommand = new RelayCommand(_ => Exit());
        }

        public ICommand HomeCommand { get; }

        public IView HomeView { get; set;}

        public bool IsHomeVisible { get; private set; }

        public ICommand LogoutCommand { get; }
        public ICommand ExitCommand { get; }

        private void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Home()
        {
            navigator.NavigateTo(HomeView);
        }
        
        private void Logout()
        {
            authenticationSession.Logout();
            navigator.ReturnToLogin();
        }
        
        [SuppressPropertyChangedWarnings]
        private void OnViewChanged(object sender, EventArgs.ViewEventArgs e)
        {
            this.IsHomeVisible = e.View != HomeView;
        }
    }
}