using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductionManager.Core.Abstractions;

namespace Solarponics.ProductionManager.ViewModels
{
    public class GrainSpawnMixViewModel : ViewModelBase, IGrainSpawnMixViewModel
    {
        private readonly IDialogBox dialogBox;
        private readonly IGrainSpawnApiClient grainSpawnApiClient;
        private readonly IHardwareProvider hardwareProvider;
        private readonly ICultureApiClient cultureApiClient;
        private readonly IBannerNotifier bannerNotifier;

        public GrainSpawnMixViewModel(IDialogBox dialogBox, IGrainSpawnApiClient grainSpawnApiClient, ICultureApiClient cultureApiClient, IHardwareProvider hardwareProvider, ILoggedInButtonsViewModel loggedInButtonsViewModel, IBannerNotifier bannerNotifier)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.dialogBox = dialogBox;
            this.bannerNotifier = bannerNotifier;
            this.grainSpawnApiClient = grainSpawnApiClient;
            this.cultureApiClient = cultureApiClient;
            this.hardwareProvider = hardwareProvider;
            this.ConfirmCommand = new RelayCommand(_ => this.Confirm());
            this.CancelCommand = new RelayCommand(_ => this.Cancel());
            this.IsUiEnabled = true;
        }

        public bool IsUiEnabled { get; private set; }

        public bool IsConfirmEnabled => GrainSpawn != null;
        public bool IsCancelEnabled => GrainSpawn != null;
        public DateTime Date { get; set; }
        private GrainSpawn GrainSpawn { get; set; }

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
                this.dialogBox.Show("No barcode scanner attached, mixing won't work");
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

            try
            {
                this.IsUiEnabled = false;
                await this.grainSpawnApiClient.Mix(this.GrainSpawn.Id, new Models.WebApi.GrainSpawnAddMixRequest
                {
                    Notes = Notes,
                    Date = this.Date
                });

                this.ResetUi();
                this.bannerNotifier.DisplayMessage($"Confirmed grain spawn as mixed on {this.Date.ToShortDateString()}");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was an unexpected problem confirming the mix.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void ResetUi()
        {
            this.GrainSpawn = null;
            this.Notes = null;
            this.ActionMessage = "Scan the grain spawn.";
        }

        private async void OnBarcodeRead(object sender, Data.BarcodeReadEventArgs e)
        {
            if (this.GrainSpawn != null || !this.IsUiEnabled)
            {
                return;
            }

            var barcode = e.Barcode;
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

            if (!grainSpawn.CultureId.HasValue)
            {
                this.dialogBox.Show("This grain spawn has not been innoculated so cannot be mixed");
                return;
            }

            this.GrainSpawn = grainSpawn;
            this.ActionMessage = "Spawn is " + grainSpawn.RecipeName;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Check date and enter notes";
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Press Confirm or Cancel to continue";
        }
    }
}