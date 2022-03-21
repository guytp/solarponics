using System;
using System.Diagnostics;
using System.Windows;
using Autofac;
using Microsoft.Extensions.Configuration;
using Serilog;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Core.Data;
using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager
{
    public class App : Application
    {
        public App()
        {
            Startup += OnStartup;
        }

        public IComponentLocator ComponentLocator { get; private set; }

        public IHardwareProvider HardwareProvider { get; private set; }

        internal static void HandleUnhandledError(Exception ex)
        {
            var message = "A fatal error occurred." + Environment.NewLine + Environment.NewLine + ex;
            Log.Error(ex, "Fatal application error");
            MessageBox.Show(message);
            Trace.WriteLine(message);
            Console.WriteLine(message);
            Environment.Exit(1);
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                ShutdownMode = ShutdownMode.OnMainWindowClose;

                var configBuilder = new ConfigurationBuilder()
                    .AddJsonFile("AppSettings.json", false, true)
                    .AddEnvironmentVariables();
                var configuration = configBuilder.Build();
                var appSettings = new ProductionManagerSettings();
                configuration.Bind("ProductionManager", appSettings);

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                var autoFacModule = new AutoFacModule(appSettings);
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterModule(autoFacModule);
                var container = containerBuilder.Build();
                ComponentLocator = container.Resolve<IComponentLocator>();

                HardwareProvider = this.ComponentLocator.ResolveComponent<IHardwareProvider>();
                await HardwareProvider.Start();

                var mainWindow = container.Resolve<IMainWindow>();
                mainWindow.Show();
                MainWindow = (Window) mainWindow;
            }
            catch (Exception ex)
            {
                HandleUnhandledError(ex);
            }
        }
    }
}