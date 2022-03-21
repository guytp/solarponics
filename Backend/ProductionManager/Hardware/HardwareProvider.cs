using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using System;
using System.Threading.Tasks;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;

namespace Solarponics.ProductionManager.Hardware
{
    public class HardwareProvider : IHardwareProvider
    {
        private HardwareSettings hardwareSettings;
        private readonly IDriverProvider driverProvider;
        private readonly IHardwareApiClient apiClient;
        private readonly IDialogBox dialogBox;

        public IBarcodeScanner BarcodeScanner { get; private set; }

        public IScale Scale { get; private set; }

        public ILabelPrinter LabelPrinterSmall { get; private set; }
        public ILabelPrinter LabelPrinterLarge { get; private set; }

        public HardwareProvider(IHardwareApiClient apiClient, IDriverProvider driverProvider, IDialogBox dialogBox)
        {
            this.apiClient = apiClient;
            this.driverProvider = driverProvider;
            this.dialogBox = dialogBox;
        }

        public async Task Start()
        {
            Stop();
            try
            {
                this.hardwareSettings = await this.apiClient.GetSettings(Environment.MachineName) ?? new HardwareSettings();
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to load hardare settings from backend.  No harwdare will be available.  Restart the app and if this continues contact support.", ex);
                return;
            }
            
            if (this.hardwareSettings.BarcodeScanner != null)
            {
                try
                { 
                    this.BarcodeScanner = this.driverProvider.Get<IBarcodeScanner>  (this.hardwareSettings.BarcodeScanner);
                    this.BarcodeScanner.Start();
                }
                catch (Exception ex)
                {
                    this.dialogBox.Show("Failed to start barcode scanner.  The barcode scanner won't be availblle.  Ensure it is turned on and connected then restart the app.  If this problem continues, please contact support.", ex);
                    return;
                }
            }
            if (this.hardwareSettings.Scale != null)
            {
                try
                {
                    this.Scale = this.driverProvider.Get<IScale>(this.hardwareSettings.Scale);
                    this.Scale.Start();
                }
                catch (Exception ex)
                {
                    this.dialogBox.Show("Failed to start scales.  The scales won't be availblle.  Ensure they are turned on and connected then restart the app.  If this problem continues, please contact support.", ex);
                    return;
                }
            }
            if (this.hardwareSettings.LabelPrinterSmall != null)
            {
                try
                { 
                    this.LabelPrinterSmall = this.driverProvider.Get<ILabelPrinter>(this.hardwareSettings.LabelPrinterSmall);
                    this.LabelPrinterSmall.Start();
                }
                catch (Exception ex)
                {
                    this.dialogBox.Show("Failed to start small label printer.  The printer won't be availblle.  Ensure it is turned on and connected then restart the app.  If this problem continues, please contact support.", ex);
                    return;
                }
            }
            if (this.hardwareSettings.LabelPrinterLarge != null)
            {
                try
                { 
                    this.LabelPrinterLarge = this.driverProvider.Get<ILabelPrinter>(this.hardwareSettings.LabelPrinterLarge);
                    this.LabelPrinterLarge.Start();
                }
                catch (Exception ex)
                {
                    this.dialogBox.Show("Failed to start large label printer.  The printer won't be availblle.  Ensure it is turned on and connected then restart the app.  If this problem continues, please contact support.", ex);
                    return;
                }
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