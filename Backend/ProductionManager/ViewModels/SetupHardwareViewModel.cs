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
            this.LabelPrintCommand = new RelayCommand(_ => LabelTest());
            this.serialDeviceSettingsViewModelFactory = serialDeviceSettingsViewModelFactory;
            this.printerSettingsViewModelFactory = printerSettingsViewModelFactory;
            this.dialogBox = dialogBox;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
        public ICommand LabelPrintCommand { get; }
        public bool IsLabelTestEnabled { get; private set;}

        public bool IsSaveEnabled => (this.IsBarcodeScannerUpdated || this.IsLabelPrinterUpdated || this.IsScaleUpdated) && (this.BarcodeScanner.IsValid && this.LabelPrinter.IsValid && this.Scale.IsValid);

        private bool IsBarcodeScannerUpdated => (this.BarcodeScanner != null) && ((this.hardwareSettings?.BarcodeScanner == null && !this.BarcodeScanner.IsReset) || (this.hardwareSettings?.BarcodeScanner != null && this.BarcodeScanner.IsReset));

        private bool IsLabelPrinterUpdated => (this.LabelPrinter != null) && ((this.hardwareSettings?.LabelPrinter == null && !this.LabelPrinter.IsReset) || (this.hardwareSettings?.LabelPrinter != null && this.LabelPrinter.IsReset));

        private bool IsScaleUpdated => (this.Scale != null) && ((this.hardwareSettings?.Scale == null && !this.Scale.IsReset) || (this.hardwareSettings?.Scale != null && this.Scale.IsReset));

        public ICommand SaveCommand { get; }
        public ISerialDeviceSettingsViewModel BarcodeScanner { get; private set; }
        public IPrinterSettingsViewModel LabelPrinter { get; private set; }
        public ISerialDeviceSettingsViewModel Scale { get; private set; }
        public bool IsUiEnabled { get; private set; }

        public async override Task OnShow()
        {
            this.IsUiEnabled = false;

            this.hardwareSettings = await this.apiClient.GetSettings(Environment.MachineName);
            this.BarcodeScanner = serialDeviceSettingsViewModelFactory.Create(hardwareSettings.BarcodeScanner, SerialDeviceType.BarcodeScanner);
            this.LabelPrinter = printerSettingsViewModelFactory.Create(hardwareSettings.LabelPrinter);
            this.Scale = serialDeviceSettingsViewModelFactory.Create(hardwareSettings.Scale, SerialDeviceType.Scale);

            this.BarcodeScanner.PropertyChanged += OnSettingPropertyChanged;
            this.LabelPrinter.PropertyChanged += OnSettingPropertyChanged;
            this.Scale.PropertyChanged += OnSettingPropertyChanged;
            await this.BarcodeScanner.OnShow();
            await this.LabelPrinter.OnShow();
            await this.Scale.OnShow();
            
            this.IsLabelTestEnabled = this.hardwareProvider.LabelPrinter != null;

            if (this.hardwareProvider.BarcodeScanner != null)
                this.hardwareProvider.BarcodeScanner.BarcodeRead += OnBarcodeRead;
            if (this.hardwareProvider.Scale != null)
                this.hardwareProvider.Scale.WeightRead += OnWeightRead;

            this.IsUiEnabled = true;
        }

        public async override Task OnHide()
        {
            this.BarcodeScanner.PropertyChanged -= OnSettingPropertyChanged;
            this.LabelPrinter.PropertyChanged -= OnSettingPropertyChanged;
            this.Scale.PropertyChanged -= OnSettingPropertyChanged;
            if (this.hardwareProvider.BarcodeScanner != null)
                this.hardwareProvider.BarcodeScanner.BarcodeRead -= OnBarcodeRead;
            if (this.hardwareProvider.Scale != null)
                this.hardwareProvider.Scale.WeightRead -= OnWeightRead;
            
            await this.BarcodeScanner.OnHide();
            await this.LabelPrinter.OnHide();
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

                if (!this.LabelPrinter.IsReset)
                {
                    await this.apiClient.SetLabelPrinter(Environment.MachineName, new LabelPrinterSettings
                    {
                        DriverName = this.LabelPrinter.DriverName,
                        QueueName = this.LabelPrinter.QueueName
                    });
                }
                else if (this.LabelPrinter.IsReset && this.hardwareSettings.LabelPrinter != null)
                {
                    await this.apiClient.RemoveLabelPrinter(Environment.MachineName);
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

        private void LabelTest()
        {
            if (this.hardwareProvider.LabelPrinter == null)
            {
                this.dialogBox.Show("No label printer configured.  If you've changed settings you must hit save first.");
                return;
            }

            this.hardwareProvider.LabelPrinter.Print(new Data.LabelDefinition("Testing", "ABC12345"));
        }
    }
}