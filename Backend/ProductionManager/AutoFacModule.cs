using Autofac;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Factories;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.Modules;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.ApiClient;
using Solarponics.ProductionManager.Data;
using Solarponics.ProductionManager.Domain;
using Solarponics.ProductionManager.Factories;
using Solarponics.ProductionManager.Hardware;
using Solarponics.ProductionManager.Modules;
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
            builder.RegisterType<MainMenuItemCategoryView>().As<IMainMenuItemCategoryView>().SingleInstance();
            builder.RegisterType<MainMenuItemCategoryViewModel>().As<IMainMenuItemCategoryViewModel>().SingleInstance();
            builder.RegisterType<LoggedInButtonsView>().As<ILoggedInButtonsView>().SingleInstance();
            builder.RegisterType<LoggedInButtonsViewModel>().As<ILoggedInButtonsViewModel>().SingleInstance();
            builder.RegisterType<StatusBarViewModel>().As<IStatusBarViewModel>().SingleInstance();

            builder.RegisterType<AuthenticationSession>().As<IAuthenticationSession>().SingleInstance();

            builder.RegisterType<DialogBoxWindowViewModelFactory>().As<IDialogBoxWindowViewModelFactory>().SingleInstance();
            builder.RegisterType<PrinterSettingsViewModelFactory>().As<IPrinterSettingsViewModelFactory>().SingleInstance();
            builder.RegisterType<DialogBoxWindowFactory>().As<IDialogBoxWindowFactory>().SingleInstance();
            builder.RegisterType<SerialDeviceSettingsViewModelFactory>().As<ISerialDeviceSettingsViewModelFactory>().SingleInstance();

            builder.RegisterType<DialogBox>().As<IDialogBox>();

            builder.RegisterType<AuthenticationApiClient>().As<IAuthenticationApiClient>();
            builder.RegisterType<SupplierApiClient>().As<ISupplierApiClient>();
            builder.RegisterType<RecipeApiClient>().As<IRecipeApiClient>();
            builder.RegisterType<HardwareApiClient>().As<IHardwareApiClient>();
            builder.RegisterType<CultureApiClient>().As<ICultureApiClient>();

            builder.RegisterType<CultureBookInView>().As<ICultureBookInView>().SingleInstance();
            builder.RegisterType<CultureBookInViewModel>().As<ICultureBookInViewModel>().SingleInstance();
            builder.RegisterType<CultureAgarLiquidPrepView>().As<ICultureAgarLiquidPrepView>().SingleInstance();
            builder.RegisterType<CultureAgarLiquidPrepViewModel>().As<ICultureAgarLiquidPrepViewModel>().SingleInstance();
            builder.RegisterType<CultureInnoculateView>().As<ICultureInnoculateView>().SingleInstance();
            builder.RegisterType<CultureInnoculateViewModel>().As<ICultureInnoculateViewModel>().SingleInstance();
            builder.RegisterType<CultureModule>().As<ICultureModule>().SingleInstance();

            builder.RegisterType<SetupSupplierView>().As<ISetupSupplierView>().SingleInstance();
            builder.RegisterType<SetupSupplierViewModel>().As<ISetupSupplierViewModel>().SingleInstance();
            builder.RegisterType<SetupRecipeView>().As<ISetupRecipeView>().SingleInstance();
            builder.RegisterType<SetupRecipeViewModel>().As<ISetupRecipeViewModel>().SingleInstance();
            builder.RegisterType<SetupHardwareView>().As<ISetupHardwareView>().SingleInstance();
            builder.RegisterType<SetupHardwareViewModel>().As<ISetupHardwareViewModel>().SingleInstance();
            builder.RegisterType<SetupModule>().As<ISetupModule>().SingleInstance();
            
            builder.RegisterType<DriverProvider>().As<IDriverProvider>().SingleInstance();
            builder.RegisterType<HardwareProvider>().As<IHardwareProvider>().SingleInstance();

            builder.RegisterType<ModuleProvider>().As<IModuleProvider>().SingleInstance();
            builder.RegisterType<ModuleContainer>().As<IModuleContainer>().SingleInstance();
        }
    }
}