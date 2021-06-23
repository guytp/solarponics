using Autofac;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.ApiClient;
using Solarponics.ProductionManager.Data;
using Solarponics.ProductionManager.Domain;
using Solarponics.ProductionManager.Factories;
using Solarponics.ProductionManager.ViewModels;
using Solarponics.ProductionManager.Views;

namespace Solarponics.ProductionManager
{
    public class AutoFacModule : Module
    {
        private readonly ProductionManagerSettings settings;

        public AutoFacModule(ProductionManagerSettings settings)
        {
            this.settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(settings).SingleInstance();
            builder.RegisterType<ComponentLocator>().As<IComponentLocator>().SingleInstance();
            builder.RegisterType<MainWindow>().As<IMainWindow>().SingleInstance();
            builder.RegisterType<MainWindowViewModel>().As<IMainWindowViewModel>().SingleInstance();
            builder.RegisterType<Navigator>().As<INavigator>().SingleInstance();
            builder.RegisterType<LoginView>().As<ILoginView>().SingleInstance();
            builder.RegisterType<LoginViewModel>().As<ILoginViewModel>().SingleInstance();
            builder.RegisterType<LoggedInView>().As<ILoggedInView>().SingleInstance();
            builder.RegisterType<LoggedInViewModel>().As<ILoggedInViewModel>().SingleInstance();
            builder.RegisterType<StatusBarViewModel>().As<IStatusBarViewModel>().SingleInstance();
            builder.RegisterType<AuthenticationSession>().As<IAuthenticationSession>().SingleInstance();
            builder.RegisterType<DialogBoxWindowViewModelFactory>().As<IDialogBoxWindowViewModelFactory>();
            builder.RegisterType<DialogBoxWindowFactory>().As<IDialogBoxWindowFactory>();
            builder.RegisterType<DialogBox>().As<IDialogBox>();
            builder.RegisterType<AuthenticationApiClient>().As<IAuthenticationApiClient>();
        }
    }
}