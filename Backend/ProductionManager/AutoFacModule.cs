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
            builder.RegisterType<SensorReadingApiClient>().As<ISensorReadingApiClient>();
            builder.RegisterType<SensorModuleApiClient>().As<ISensorModuleApiClient>();
            builder.RegisterType<LocationApiClient>().As<ILocationApiClient>();
            builder.RegisterType<AutoclaveApiClient>().As<IAutoclaveApiClient>();
            builder.RegisterType<ShelfApiClient>().As<IShelfApiClient>();
            builder.RegisterType<GrainSpawnApiClient>().As<IGrainSpawnApiClient>();
            builder.RegisterType<WasteReasonApiClient>().As<IWasteReasonApiClient>();

            builder.RegisterType<GrainSpawnCreateView>().As<IGrainSpawnCreateView>().SingleInstance();
            builder.RegisterType<GrainSpawnCreateViewModel>().As<IGrainSpawnCreateViewModel>().SingleInstance();
            builder.RegisterType<CultureAgarLiquidPrepView>().As<ICultureAgarLiquidPrepView>().SingleInstance();
            builder.RegisterType<CultureAgarLiquidPrepViewModel>().As<ICultureAgarLiquidPrepViewModel>().SingleInstance();
            builder.RegisterType<CultureInnoculateView>().As<ICultureInnoculateView>().SingleInstance();
            builder.RegisterType<CultureInnoculateViewModel>().As<ICultureInnoculateViewModel>().SingleInstance();
            builder.RegisterType<CultureListView>().As<ICultureListView>().SingleInstance();
            builder.RegisterType<CultureListViewModel>().As<ICultureListViewModel>().SingleInstance();
            builder.RegisterType<CultureBookInViewModel>().As<ICultureBookInViewModel>().SingleInstance();
            builder.RegisterType<CultureBookInView>().As<ICultureBookInView>().SingleInstance();
            builder.RegisterType<CultureModule>().As<ICultureModule>().SingleInstance();

            builder.RegisterType<GrainSpawnCreateView>().As<IGrainSpawnCreateView>().SingleInstance();
            builder.RegisterType<GrainSpawnCreateViewModel>().As<IGrainSpawnCreateViewModel>().SingleInstance();
            builder.RegisterType<GrainSpawnListView>().As<IGrainSpawnListView>().SingleInstance();
            builder.RegisterType<GrainSpawnListViewModel>().As<IGrainSpawnListViewModel>().SingleInstance();
            builder.RegisterType<GrainSpawnInnoculateView>().As<IGrainSpawnInnoculateView>().SingleInstance();
            builder.RegisterType<GrainSpawnInnoculateViewModel>().As<IGrainSpawnInnoculateViewModel>().SingleInstance();
            builder.RegisterType<GrainSpawnShelfPlaceView>().As<IGrainSpawnShelfPlaceView>().SingleInstance();
            builder.RegisterType<GrainSpawnShelfPlaceViewModel>().As<IGrainSpawnShelfPlaceViewModel>().SingleInstance();
            builder.RegisterType<GrainSpawnModule>().As<IGrainSpawnModule>().SingleInstance();

            builder.RegisterType<EnvironmentSensorReadingsCurrentView>().As<IEnvironmentSensorReadingsCurrentView>().SingleInstance();
            builder.RegisterType<EnvironmentSensorReadingsCurrentViewModel>().As<IEnvironmentSensorReadingsCurrentViewModel>().SingleInstance();
            builder.RegisterType<EnvironmentSensorReadingsAggregateView>().As<IEnvironmentSensorReadingsAggregateView>().SingleInstance();
            builder.RegisterType<EnvironmentSensorReadingsAggregateViewModel>().As<IEnvironmentSensorReadingsAggregateViewModel>().SingleInstance();
            builder.RegisterType<EnvironmentModule>().As<IEnvironmentModule>().SingleInstance();

            builder.RegisterType<SetupSupplierView>().As<ISetupSupplierView>().SingleInstance();
            builder.RegisterType<SetupSupplierViewModel>().As<ISetupSupplierViewModel>().SingleInstance();
            builder.RegisterType<SetupRecipeView>().As<ISetupRecipeView>().SingleInstance();
            builder.RegisterType<SetupRecipeViewModel>().As<ISetupRecipeViewModel>().SingleInstance();
            builder.RegisterType<SetupHardwareView>().As<ISetupHardwareView>().SingleInstance();
            builder.RegisterType<SetupHardwareViewModel>().As<ISetupHardwareViewModel>().SingleInstance();
            builder.RegisterType<SetupRoomsAndLocationsViewModel>().As<ISetupRoomsAndLocationsViewModel>().SingleInstance();
            builder.RegisterType<SetupRoomsAndLocationsView>().As<ISetupRoomsAndLocationsView>().SingleInstance();
            builder.RegisterType<SetupSensorModuleModbusTcpViewModel>().As<ISetupSensorModuleModbusTcpViewModel>().SingleInstance();
            builder.RegisterType<SetupSensorModuleModbusTcpView>().As<ISetupSensorModuleModbusTcpView>().SingleInstance();
            builder.RegisterType<SetupAutoclavesView>().As<ISetupAutoclavesView>().SingleInstance();
            builder.RegisterType<SetupAutoclavesViewModel>().As<ISetupAutoclavesViewModel>().SingleInstance();
            builder.RegisterType<SetupShelvesView>().As<ISetupShelvesView>().SingleInstance();
            builder.RegisterType<SetupShelvesViewModel>().As<ISetupShelvesViewModel>().SingleInstance();
            builder.RegisterType<SetupWasteReasonsView>().As<ISetupWasteReasonsView>().SingleInstance();
            builder.RegisterType<SetupWasteReasonsViewModel>().As<ISetupWasteReasonsViewModel>().SingleInstance();
            builder.RegisterType<SetupModule>().As<ISetupModule>().SingleInstance();
            
            builder.RegisterType<DriverProvider>().As<IDriverProvider>().SingleInstance();
            builder.RegisterType<HardwareProvider>().As<IHardwareProvider>().SingleInstance();

            builder.RegisterType<ModuleProvider>().As<IModuleProvider>().SingleInstance();
            builder.RegisterType<ModuleContainer>().As<IModuleContainer>().SingleInstance();
        }
    }
}