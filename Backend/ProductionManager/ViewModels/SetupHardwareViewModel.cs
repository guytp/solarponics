using PropertyChanged;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Factories;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupHardwareViewModel : ViewModelBase, ISetupHardwareViewModel
    {
        private readonly IHardwareProvider hardwareProvider;
        private readonly IHardwareApiClient apiClient;
        private HardwareSettings hardwareSettings;
        private readonly ISerialDeviceSettingsViewModelFactory serialDeviceSettingsViewModelFactory;
        private readonly IPrinterSettingsViewModelFactory printerSettingsViewModelFactory;
        private readonly IDialogBox dialogBox;

        public SetupHardwareViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, IHardwareProvider hardwareProvider, IHardwareApiClient apiClient, ISerialDeviceSettingsViewModelFactory serialDeviceSettingsViewModelFactory, IPrinterSettingsViewModelFactory printerSettingsViewModelFactory, IDialogBox dialogBox)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.hardwareProvider = hardwareProvider;
            this.apiClient = apiClient;
            this.SaveCommand = new RelayCommand(_ => Save());
            this.LabelPrintSmallCommand = new RelayCommand(_ => LabelTestSmall());
            this.LabelPrintLargeCommand = new RelayCommand(_ => LabelTestLarge());
            this.serialDeviceSettingsViewModelFactory = serialDeviceSettingsViewModelFactory;
            this.printerSettingsViewModelFactory = printerSettingsViewModelFactory;
            this.dialogBox = dialogBox;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
        public ICommand LabelPrintSmallCommand { get; }
        public ICommand LabelPrintLargeCommand { get; }
        public bool IsLabelTestSmallEnabled { get; private set;}
        public bool IsLabelTestLargeEnabled { get; private set; }

        public bool IsSaveEnabled => (this.IsBarcodeScannerUpdated || this.IsLabelPrinterSmallUpdated || this.IsLabelPrinterLargeUpdated || this.IsScaleUpdated) && (this.BarcodeScanner.IsValid && this.LabelPrinterLarge.IsValid && this.LabelPrinterSmall.IsValid && this.Scale.IsValid);

        private bool IsBarcodeScannerUpdated => (this.BarcodeScanner != null) && ((this.hardwareSettings?.BarcodeScanner == null && !this.BarcodeScanner.IsReset) || (this.hardwareSettings?.BarcodeScanner != null && this.BarcodeScanner.IsReset));

        private bool IsLabelPrinterLargeUpdated => (this.LabelPrinterLarge != null) && ((this.hardwareSettings?.LabelPrinterLarge == null && !this.LabelPrinterLarge.IsReset) || (this.hardwareSettings?.LabelPrinterLarge != null && this.LabelPrinterLarge.IsReset));

        private bool IsLabelPrinterSmallUpdated => (this.LabelPrinterSmall != null) && ((this.hardwareSettings?.LabelPrinterSmall == null && !this.LabelPrinterSmall.IsReset) || (this.hardwareSettings?.LabelPrinterSmall != null && this.LabelPrinterSmall.IsReset));

        private bool IsScaleUpdated => (this.Scale != null) && ((this.hardwareSettings?.Scale == null && !this.Scale.IsReset) || (this.hardwareSettings?.Scale != null && this.Scale.IsReset));

        public ICommand SaveCommand { get; }
        public ISerialDeviceSettingsViewModel BarcodeScanner { get; private set; }
        public IPrinterSettingsViewModel LabelPrinterSmall { get; private set; }
        public IPrinterSettingsViewModel LabelPrinterLarge { get; private set; }
        public ISerialDeviceSettingsViewModel Scale { get; private set; }
        public bool IsUiEnabled { get; private set; }

        public async override Task OnShow()
        {
            this.IsUiEnabled = false;

            this.hardwareSettings = await this.apiClient.GetSettings(Environment.MachineName);
            this.BarcodeScanner = serialDeviceSettingsViewModelFactory.Create(hardwareSettings.BarcodeScanner, SerialDeviceType.BarcodeScanner);
            this.LabelPrinterSmall = printerSettingsViewModelFactory.Create(hardwareSettings.LabelPrinterSmall);
            this.LabelPrinterLarge = printerSettingsViewModelFactory.Create(hardwareSettings.LabelPrinterLarge);
            this.Scale = serialDeviceSettingsViewModelFactory.Create(hardwareSettings.Scale, SerialDeviceType.Scale);

            this.BarcodeScanner.PropertyChanged += OnSettingPropertyChanged;
            this.LabelPrinterSmall.PropertyChanged += OnSettingPropertyChanged;
            this.LabelPrinterLarge.PropertyChanged += OnSettingPropertyChanged;
            this.Scale.PropertyChanged += OnSettingPropertyChanged;
            await this.BarcodeScanner.OnShow();
            await this.LabelPrinterSmall.OnShow();
            await this.LabelPrinterLarge.OnShow();
            await this.Scale.OnShow();

            this.IsLabelTestSmallEnabled = this.hardwareProvider.LabelPrinterSmall != null;
            this.IsLabelTestLargeEnabled = this.hardwareProvider.LabelPrinterLarge != null;

            if (this.hardwareProvider.BarcodeScanner != null)
                this.hardwareProvider.BarcodeScanner.BarcodeRead += OnBarcodeRead;
            if (this.hardwareProvider.Scale != null)
                this.hardwareProvider.Scale.WeightRead += OnWeightRead;

            this.IsUiEnabled = true;
        }

        public async override Task OnHide()
        {
            this.BarcodeScanner.PropertyChanged -= OnSettingPropertyChanged;
            this.LabelPrinterSmall.PropertyChanged -= OnSettingPropertyChanged;
            this.LabelPrinterLarge.PropertyChanged -= OnSettingPropertyChanged;
            this.Scale.PropertyChanged -= OnSettingPropertyChanged;
            if (this.hardwareProvider.BarcodeScanner != null)
                this.hardwareProvider.BarcodeScanner.BarcodeRead -= OnBarcodeRead;
            if (this.hardwareProvider.Scale != null)
                this.hardwareProvider.Scale.WeightRead -= OnWeightRead;
            
            await this.BarcodeScanner.OnHide();
            await this.LabelPrinterSmall.OnHide();
            await this.LabelPrinterLarge.OnHide();
            await this.Scale.OnHide();
        }

        private async void Save()
        {
            if (!IsSaveEnabled)
                return;

            try
            {
                this.IsUiEnabled = false;

                if (!this.BarcodeScanner.IsReset)
                {
                    await this.apiClient.SetBarcodeScanner(Environment.MachineName, new BarcodeScannerSettings
                    {
                        BaudRate = this.BarcodeScanner.BaudRate.Value,
                        DataBits = this.BarcodeScanner.DataBits.Value,
                        DriverName = this.BarcodeScanner.DriverName,
                        Parity = this.BarcodeScanner.Parity.Value,
                        SerialPort = this.BarcodeScanner.SerialPort,
                        StopBits = this.BarcodeScanner.StopBits.Value
                    });
                }
                else if (this.BarcodeScanner.IsReset && this.hardwareSettings.BarcodeScanner != null)
                {
                    await this.apiClient.RemoveBarcodeScanner(Environment.MachineName);
                }

                if (!this.LabelPrinterSmall.IsReset)
                {
                    await this.apiClient.SetLabelPrinterSmall(Environment.MachineName, new LabelPrinterSettings
                    {
                        DriverName = this.LabelPrinterSmall.DriverName,
                        QueueName = this.LabelPrinterSmall.QueueName
                    });
                }
                else if (this.LabelPrinterSmall.IsReset && this.hardwareSettings.LabelPrinterSmall != null)
                {
                    await this.apiClient.RemoveLabelPrinterSmall(Environment.MachineName);
                }

                if (!this.LabelPrinterLarge.IsReset)
                {
                    await this.apiClient.SetLabelPrinterLarge(Environment.MachineName, new LabelPrinterSettings
                    {
                        DriverName = this.LabelPrinterLarge.DriverName,
                        QueueName = this.LabelPrinterLarge.QueueName
                    });
                }
                else if (this.LabelPrinterLarge.IsReset && this.hardwareSettings.LabelPrinterLarge != null)
                {
                    await this.apiClient.RemoveLabelPrinterLarge(Environment.MachineName);
                }

                if (!this.Scale.IsReset)
                {
                    await this.apiClient.SetScale(Environment.MachineName, new ScaleSettings
                    {
                        BaudRate = this.Scale.BaudRate.Value,
                        DataBits = this.Scale.DataBits.Value,
                        DriverName = this.Scale.DriverName,
                        Parity = this.Scale.Parity.Value,
                        SerialPort = this.Scale.SerialPort,
                        StopBits = this.Scale.StopBits.Value
                    });
                }
                else if (this.Scale.IsReset && this.hardwareSettings.Scale != null)
                {
                    // TODO: Remove settings
                    await this.apiClient.RemoveScale(Environment.MachineName);
                }

                // At end update hardware service then call OnShow() again
                this.hardwareProvider.Stop();
                await this.hardwareProvider.Start();

                await OnHide();
                await OnShow();

                this.dialogBox.Show("Updated hardware settings, changes should take immediate effect.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to update hardware settings.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        [SuppressPropertyChangedWarnings]
        private void OnSettingPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(IsSaveEnabled));
        }
        
        private void OnWeightRead(object sender, Data.WeightReadEventArgs e)
        {
            this.dialogBox.Show("Scale weighed: " + e.Weight);
        }

        private void OnBarcodeRead(object sender, Data.BarcodeReadEventArgs e)
        {
            this.dialogBox.Show("Barcode scanned: " + e.Barcode);
        }

        private void LabelTestSmall()
        {
            if (this.hardwareProvider.LabelPrinterSmall == null)
            {
                this.dialogBox.Show("No small label printer configured.  If you've changed settings you must hit save first.");
                return;
            }

            this.hardwareProvider.LabelPrinterSmall.Print(new LabelDefinitions.LabelDefinition("Testing small labels", "SM123", LabelDefinitions.BarcodeSize.Small, textSize: LabelDefinitions.TextSize.Small));
            this.hardwareProvider.LabelPrinterSmall.Print(new LabelDefinitions.LabelDefinition(null, "SM123", LabelDefinitions.BarcodeSize.Small));
            this.hardwareProvider.LabelPrinterSmall.Print(new LabelDefinitions.LabelDefinition("This is a test small label with lots\r\nof text on it that should wrap\r\non to multiple lines within the\r\nlabel.", textSize: LabelDefinitions.TextSize.Small));
        }

        private void LabelTestLarge()
        {
            if (this.hardwareProvider.LabelPrinterLarge == null)
            {
                this.dialogBox.Show("No large label printer configured.  If you've changed settings you must hit save first.");
                return;
            }

            this.hardwareProvider.LabelPrinterLarge.Print(new LabelDefinitions.LabelDefinition("Testing large labels", "LG123", LabelDefinitions.BarcodeSize.Medium));
            this.hardwareProvider.LabelPrinterLarge.Print(new LabelDefinitions.LabelDefinition(null, "LG123", LabelDefinitions.BarcodeSize.Small));
            this.hardwareProvider.LabelPrinterLarge.Print(new LabelDefinitions.LabelDefinition("This is a test large label with lots\r\nof text on it that should wrap\r\non to multiple lines within the\r\nlabel."));
        }
    }
}