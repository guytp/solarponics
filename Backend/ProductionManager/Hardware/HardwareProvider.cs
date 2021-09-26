using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using System;
using System.Threading.Tasks;

namespace Solarponics.ProductionManager.Hardware
{
    public class HardwareProvider : IHardwareProvider
    {
        private HardwareSettings hardwareSettings;
        private readonly IDriverProvider driverProvider;
        private readonly IHardwareApiClient apiClient;
        
        public IBarcodeScanner BarcodeScanner { get; private set; }

        public IScale Scale { get; private set; }

        public ILabelPrinter LabelPrinter { get; private set; }

        public HardwareProvider(IHardwareApiClient apiClient, IDriverProvider driverProvider)
        {
            this.apiClient = apiClient;
            this.driverProvider = driverProvider;
        }

        public async Task Start()
        {
            Stop();
            this.hardwareSettings = await this.apiClient.GetSettings(Environment.MachineName) ?? new HardwareSettings();
            
            if (this.hardwareSettings.BarcodeScanner != null)
            {
                this.BarcodeScanner = this.driverProvider.Get<IBarcodeScanner>(this.hardwareSettings.BarcodeScanner);
                this.BarcodeScanner.Start();
            }
            if (this.hardwareSettings.Scale != null)
            {
                this.Scale = this.driverProvider.Get<IScale>(this.hardwareSettings.Scale);
                this.Scale.Start();
            }
            if (this.hardwareSettings.LabelPrinter != null)
            {
                this.LabelPrinter = this.driverProvider.Get<ILabelPrinter>(this.hardwareSettings.LabelPrinter);
                this.LabelPrinter.Start();
            }
        }

        public void Stop()
        {
            if (this.BarcodeScanner is IDisposable d)
                d?.Dispose();
            if (this.Scale is IDisposable scales)
                scales?.Dispose();
            if (this.LabelPrinter is IDisposable labelPrinter)
                labelPrinter?.Dispose();
            
            this.BarcodeScanner = null;
            this.Scale = null;
            this.LabelPrinter = null;
            this.hardwareSettings = null;
        }
    }
}