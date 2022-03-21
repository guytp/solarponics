using Solarponics.ProductionManagerMobile.Abstractions.ViewModels;
using Solarponics.ProductionManagerMobile.Domain;
using Solarponics.ProductionManagerMobile.Services;
using Solarponics.ProductionManagerMobile.ViewModels;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;
using Solarponics.ProductionManager.Core.ApiClient;
using Solarponics.ProductionManager.Core.Data;
using Solarponics.ProductionManager.Core.Domain;
using Xamarin.Forms;

namespace Solarponics.ProductionManagerMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            var settings = new ProductionManagerSettings
            {
                ApiBaseUrl = "http://10.200.0.33:4202/"
            };
            var authenticationApiClient = new AuthenticationApiClient(settings);
            var dialogBox = new DialogBox();
            var authenticationSession = new AuthenticationSession(authenticationApiClient);
            DependencyService.RegisterSingleton(settings);
            DependencyService.RegisterSingleton<IAuthenticationApiClient>(authenticationApiClient);
            DependencyService.RegisterSingleton<IDialogBox>(dialogBox);
            DependencyService.RegisterSingleton<IAuthenticationSession>(authenticationSession);
            DependencyService.RegisterSingleton<ILoginViewModel>(new LoginViewModel(dialogBox, authenticationApiClient, authenticationSession));
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
