using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;

namespace Solarponics.ProductionManager.ViewModels
{
    public class GrainSpawnInnoculateViewModel : ViewModelBase, IGrainSpawnInnoculateViewModel
    {
        private readonly IDialogBox dialogBox;
        private readonly IGrainSpawnApiClient grainSpawnApiClient;
        private readonly IHardwareProvider hardwareProvider;
        private readonly ICultureApiClient cultureApiClient;
        private readonly IBannerNotifier bannerNotifier;

        public GrainSpawnInnoculateViewModel(IDialogBox dialogBox, IGrainSpawnApiClient grainSpawnApiClient, ICultureApiClient cultureApiClient, IHardwareProvider hardwareProvider, ILoggedInButtonsViewModel loggedInButtonsViewModel, IBannerNotifier bannerNotifier)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.bannerNotifier = bannerNotifier;
            this.dialogBox = dialogBox;
            this.grainSpawnApiClient = grainSpawnApiClient;
            this.cultureApiClient = cultureApiClient;
            this.hardwareProvider = hardwareProvider;
            this.ConfirmCommand = new RelayCommand(_ => this.Confirm());
            this.CancelCommand = new RelayCommand(_ => this.Cancel());
            this.IsUiEnabled = true;
        }

        public bool IsUiEnabled { get; private set; }

        public bool IsConfirmEnabled => GrainSpawn != null && Culture != null;
        public bool IsCancelEnabled => GrainSpawn != null || Culture != null;
        public DateTime Date { get; set; }
        private GrainSpawn GrainSpawn { get; set; }

        private Culture Culture { get; set; }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public string Notes { get; set; }

        public string ActionMessage { get; private set; }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public override Task OnShow()
        {
            this.Date = DateTime.UtcNow;
            if (hardwareProvider.BarcodeScanner != null)
                hardwareProvider.BarcodeScanner.BarcodeRead += OnBarcodeRead;
            else
                this.dialogBox.Show("No barcode scanner attached, innoculation won't work");
            this.ResetUi();
            return Task.CompletedTask;
        }

        public override Task OnHide()
        {
            if (hardwareProvider.BarcodeScanner != null)
                hardwareProvider.BarcodeScanner.BarcodeRead -= OnBarcodeRead;
            return Task.CompletedTask;
        }

        private void Cancel()
        {
            if (!this.IsUiEnabled || !this.IsCancelEnabled)
                return;

            this.ResetUi();
        }

        private async void Confirm()
        {
            if (!this.IsUiEnabled || !this.IsConfirmEnabled)
                return;

            if (hardwareProvider?.LabelPrinterLarge == null)
            {
                this.dialogBox.Show("Unable to inoculate grain spawn without large label printer");
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                var grainSpawn = await this.grainSpawnApiClient.Innoculate(this.GrainSpawn.Id, new Models.WebApi.GrainSpawnInnoculateRequest
                {
                    AdditionalNotes = Notes,
                    CultureId = this.Culture.Id,
                    Date = this.Date
                });

                this.hardwareProvider.LabelPrinterLarge.Print(new GrainSpawnLabelDefinition(grainSpawn));
                this.ResetUi();
                this.bannerNotifier.DisplayMessage("Confirmed inoculation and new label printed");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was an unexpected problem confirming the inoculation.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void ResetUi()
        {
            this.GrainSpawn = null;
            this.Culture = null;
            this.Notes = null;
            this.ActionMessage = "Scan the grain spawn.";
        }

        private async void OnBarcodeRead(object sender, Data.BarcodeReadEventArgs e)
        {
            if ((this.Culture != null && this.GrainSpawn != null) || !this.IsUiEnabled)
            {
                return;
            }

            if (this.GrainSpawn == null)
            {
                await HandleGrainSpawnScan(e.Barcode);
            }

            else if (this.Culture == null)
            {
                await HandleCultureScan(e.Barcode);
            }
        }

        private async Task HandleGrainSpawnScan(string barcode)
        {
            if (!barcode.StartsWith("GS") || barcode.Length < 3)
            {
                this.dialogBox.Show("Invalid barcode, please scan a grain spawn.");
                return;
            }

            int grainSpawnId;
            if (!int.TryParse(barcode.Substring(2), out grainSpawnId))
            {
                this.dialogBox.Show("Invalid barcode");
                return;
            }

            var grainSpawn = await this.grainSpawnApiClient.Get(grainSpawnId);
            if (grainSpawn == null)
            {
                this.dialogBox.Show("Grain spawn not found");
                return;
            }

            if (grainSpawn.CultureId.HasValue)
            {
                this.dialogBox.Show("This grain spawn has already been innoculated");
                return;
            }

            this.GrainSpawn = grainSpawn;
            this.ActionMessage = "Spawn is " + grainSpawn.RecipeName;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Please scan culture";
        }

        private async Task HandleCultureScan(string barcode)
        {
            if (!barcode.StartsWith("C") || barcode.Length < 2)
            {
                this.dialogBox.Show("Invalid barcode, please scan a culture.");
                return;
            }

            int cultureId;
            if (!int.TryParse(barcode.Substring(1), out cultureId))
            {
                this.dialogBox.Show("Invalid barcode");
                return;
            }

            var culture = await this.cultureApiClient.Get(cultureId);
            if (culture== null)
            {
                this.dialogBox.Show("Culture not found");
                return;
            }

            if (!culture.InnoculateDate.HasValue && !culture.SupplierId.HasValue)
            {
                this.dialogBox.Show("You scanned a non-innoculated culture, it must be innoculated for use in grain spawn");
                return;
            }

            this.Culture = culture;
            this.ActionMessage = "Spawn is " + this.GrainSpawn.RecipeName;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Culture is " + this.Culture.Strain;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Press Confirm or Cancel to continue";
        }
    }
}