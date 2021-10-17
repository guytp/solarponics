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

        public ILabelPrinter LabelPrinterSmall { get; private set; }
        public ILabelPrinter LabelPrinterLarge { get; private set; }

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
            if (this.hardwareSettings.LabelPrinterSmall != null)
            {
                this.LabelPrinterSmall = this.driverProvider.Get<ILabelPrinter>(this.hardwareSettings.LabelPrinterSmall);
                this.LabelPrinterSmall.Start();
            }
            if (this.hardwareSettings.LabelPrinterLarge != null)
            {
                this.LabelPrinterLarge = this.driverProvider.Get<ILabelPrinter>(this.hardwareSettings.LabelPrinterLarge);
                this.LabelPrinterLarge.Start();
            }
        }

        public void Stop()
        {
            if (this.BarcodeScanner is IDisposable d)
                d?.Dispose();
            if (this.Scale is IDisposable scales)
                scales?.Dispose();
            if (this.LabelPrinterSmall is IDisposable labelPrinterSmall)
                labelPrinterSmall?.Dispose();
            if (this.LabelPrinterLarge is IDisposable labelPrinterLarge)
                labelPrinterLarge?.Dispose();

            this.BarcodeScanner = null;
            this.Scale = null;
            this.LabelPrinterSmall = null;
            this.LabelPrinterLarge = null;
            this.hardwareSettings = null;
        }
    }
}