using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductionManager.Core.Abstractions;

namespace Solarponics.ProductionManager.ViewModels
{
    public class GrainSpawnShelfPlaceViewModel : ViewModelBase, IGrainSpawnShelfPlaceViewModel
    {
        private readonly IDialogBox dialogBox;
        private readonly IGrainSpawnApiClient grainSpawnApiClient;
        private readonly IHardwareProvider hardwareProvider;
        private readonly IShelfApiClient shelfApiClient;
        private readonly IBannerNotifier bannerNotifier;

        public GrainSpawnShelfPlaceViewModel(IDialogBox dialogBox, IGrainSpawnApiClient grainSpawnApiClient, IShelfApiClient shelfApiClient, IHardwareProvider hardwareProvider, ILoggedInButtonsViewModel loggedInButtonsViewModel, IBannerNotifier bannerNotifier)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.dialogBox = dialogBox;
            this.bannerNotifier = bannerNotifier;
            this.grainSpawnApiClient = grainSpawnApiClient;
            this.shelfApiClient = shelfApiClient;
            this.hardwareProvider = hardwareProvider;
            this.ConfirmCommand = new RelayCommand(_ => this.Confirm());
            this.CancelCommand = new RelayCommand(_ => this.Cancel());
            this.IsUiEnabled = true;
        }

        public bool IsUiEnabled { get; private set; }

        public bool IsConfirmEnabled => GrainSpawn != null && Shelf != null;
        public bool IsCancelEnabled => GrainSpawn != null || Shelf != null;

        public DateTime Date { get; set; }
        private GrainSpawn GrainSpawn { get; set; }

        private Shelf Shelf { get; set; }

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
                this.dialogBox.Show("No barcode scanner attached, shelf place won't work");
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
                var grainSpawn = await this.grainSpawnApiClient.ShelfPlace(this.GrainSpawn.Id, new Models.WebApi.GrainSpawnShelfPlaceRequest
                {
                    AdditionalNotes = Notes,
                    ShelfId = this.Shelf.Id,
                    Date = this.Date
                });

                this.ResetUi();
                this.bannerNotifier.DisplayMessage("Confirmed shelf placement");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was an unexpected problem confirming the shelf placement.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void ResetUi()
        {
            this.GrainSpawn = null;
            this.Shelf = null;
            this.Notes = null;
            this.ActionMessage = "Scan the grain spawn.";
        }

        private async void OnBarcodeRead(object sender, Data.BarcodeReadEventArgs e)
        {
            if ((this.Shelf != null && this.GrainSpawn != null) || !this.IsUiEnabled)
            {
                return;
            }

            if (this.GrainSpawn == null)
            {
                await HandleGrainSpawnScan(e.Barcode);
            }

            else if (this.Shelf == null)
            {
                await HandleShelfScan(e.Barcode);
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

            if (!grainSpawn.CultureId.HasValue)
            {
                this.dialogBox.Show("This grain spawn has not been innoculated");
                return;
            }

            if (grainSpawn.ShelfId.HasValue)
            {
                this.dialogBox.Show("Grain spawn has already been placed");
                return;
            }

            this.GrainSpawn = grainSpawn;
            this.ActionMessage = "Spawn is " + grainSpawn.RecipeName;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Please scan shelf";
        }

        private async Task HandleShelfScan(string barcode)
        {
            if (!barcode.StartsWith("SH") || barcode.Length < 3)
            {
                this.dialogBox.Show("Invalid barcode, please scan a shelf.");
                return;
            }

            int shelfId;
            if (!int.TryParse(barcode.Substring(2), out shelfId))
            {
                this.dialogBox.Show("Invalid barcode");
                return;
            }

            var shelf = (await this.shelfApiClient.Get()).FirstOrDefault(s => s.Id == shelfId);

            if (shelf == null)
            {
                this.dialogBox.Show("Shelf not found");
                return;
            }

            this.Shelf = shelf;
            this.ActionMessage = "Spawn is " + this.GrainSpawn.RecipeName;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Shelf is " + this.Shelf.Name;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Press Confirm or Cancel to continue";
        }
    }
}