using System;
using System.Windows.Input;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;

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
            IsEnabled = true;
        }

        public bool IsEnabled { get; private set; }
        public bool IsLoginEnabled => !string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(Pin);
        public string UserId { get; set; }
        public string Pin { get; set; }
        public ICommand LoginCommand { get; }

        private async void Login()
        {
            if (!IsLoginEnabled)
                return;

            IsEnabled = false;

            try
            {
                var user = await apiClient.Authenticate(new AuthenticateRequest
                {
                    UserId = short.Parse(UserId),
                    Pin = short.Parse(Pin)
                });

                UserId = null;
                Pin = null;

                if (user == null)
                {
                    dialogBox.Show("Your login details are not correct.  Please check and try again.");
                    return;
                }

                this.loggedInView.LoggedInViewModel.User = user;
                this.authenticationSession.SetUser(user);
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