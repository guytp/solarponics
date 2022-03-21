using System;
using System.Windows.Input;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Commands;

namespace Solarponics.ProductionManager.ViewModels
{
    public class LoginViewModel : ViewModelBase, ILoginViewModel
    {
        private readonly IAuthenticationApiClient apiClient;
        private readonly IDialogBox dialogBox;
        private readonly ILoggedInView loggedInView;
        private readonly INavigator navigator;
        private readonly IAuthenticationSession authenticationSession;

        public LoginViewModel(IDialogBox dialogBox, IAuthenticationApiClient apiClient, ILoggedInView loggedInView, INavigator navigator, IAuthenticationSession authenticationSession)
        {
            this.navigator = navigator;
            this.loggedInView = loggedInView;
            this.dialogBox = dialogBox;
            this.apiClient = apiClient;
            this.authenticationSession = authenticationSession;
            LoginCommand = new RelayCommand(_ => Login());
            ExitCommand = new RelayCommand(_ => Exit());
            IsEnabled = true;
        }

        public bool IsEnabled { get; private set; }
        public ICommand NumberButtonCommand { get; }
        public bool IsLoginEnabled => !string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(Pin);
        public string UserId { get; set; }
        public string Pin { get; set; }
        public ICommand LoginCommand { get; }
        public ICommand ExitCommand { get; }
        private void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private async void Login()
        {
            if (!IsLoginEnabled)
                return;

            IsEnabled = false;

            try
            {
                var request = new AuthenticateRequest
                {
                    UserId = short.Parse(UserId),
                    Pin = short.Parse(Pin)
                };
                var authenticateResponse = await apiClient.Authenticate(request);

                UserId = null;
                Pin = null;

                if (authenticateResponse == null)
                {
                    dialogBox.Show("Your login details are not correct.  Please check and try again.");
                    return;
                }

                this.loggedInView.LoggedInViewModel.User = authenticateResponse.User;
                this.authenticationSession.Login(authenticateResponse.User, authenticateResponse.AuthenticationToken, request.UserId, request.Pin);
                this.navigator.NavigateTo(this.loggedInView);
            }
            catch (Exception ex)
            {
                dialogBox.Show("There was a problem logging you in.  Please contact support if this error persists.", ex);
            }
            finally
            {
                IsEnabled = true;
            }
        }
    }
}