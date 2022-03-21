using Solarponics.ProductionManagerMobile.Abstractions.ViewModels;
using Solarponics.ProductionManagerMobile.Views;
using System;
using Solarponics.Models.WebApi;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using System.Windows.Input;
using Xamarin.Forms;

namespace Solarponics.ProductionManagerMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        private readonly IAuthenticationApiClient apiClient;
        private readonly IDialogBox dialogBox;
        private readonly IAuthenticationSession authenticationSession;

        public LoginViewModel(IDialogBox dialogBox, IAuthenticationApiClient apiClient, IAuthenticationSession authenticationSession)
        {
            this.dialogBox = dialogBox;
            this.apiClient = apiClient;
            this.authenticationSession = authenticationSession;
            LoginCommand = new Command(Login);
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

                this.authenticationSession.Login(authenticateResponse.User, authenticateResponse.AuthenticationToken, request.UserId, request.Pin);

                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
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
