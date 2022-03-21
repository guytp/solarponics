using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;

namespace Solarponics.ProductionManager.ViewModels
{
    public class FruitingBlockIncubateShelfPlaceViewModel : ViewModelBase, IFruitingBlockIncubateShelfPlaceViewModel
    {
        private readonly IDialogBox dialogBox;
        private readonly IFruitingBlockApiClient fruitingBlockApiClient;
        private readonly IHardwareProvider hardwareProvider;
        private readonly IShelfApiClient shelfApiClient;
        private readonly IBannerNotifier bannerNotifier;

        public FruitingBlockIncubateShelfPlaceViewModel(IDialogBox dialogBox, IFruitingBlockApiClient fruitingBlockApiClient, IShelfApiClient shelfApiClient, IHardwareProvider hardwareProvider, ILoggedInButtonsViewModel loggedInButtonsViewModel, IBannerNotifier bannerNotifier)
        {
            this.bannerNotifier = bannerNotifier;
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.dialogBox = dialogBox;
            this.fruitingBlockApiClient = fruitingBlockApiClient;
            this.shelfApiClient = shelfApiClient;
            this.hardwareProvider = hardwareProvider;
            this.ConfirmCommand = new RelayCommand(_ => this.Confirm());
            this.CancelCommand = new RelayCommand(_ => this.Cancel());
            this.IsUiEnabled = true;
        }

        public bool IsUiEnabled { get; private set; }

        public bool IsConfirmEnabled => FruitingBlock != null && Shelf != null;
        public bool IsCancelEnabled => FruitingBlock != null || Shelf != null;
        public DateTime Date { get; set; }
        private FruitingBlock FruitingBlock { get; set; }

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
                var fruitingBlock = await this.fruitingBlockApiClient.ShelfPlaceIncubate(this.FruitingBlock.Id, new Models.WebApi.FruitingBlockShelfPlaceRequest
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
            this.FruitingBlock = null;
            this.Shelf = null;
            this.Notes = null;
            this.ActionMessage = "Scan the fruiting block.";
        }

        private async void OnBarcodeRead(object sender, Data.BarcodeReadEventArgs e)
        {
            if ((this.Shelf != null && this.FruitingBlock != null) || !this.IsUiEnabled)
            {
                return;
            }

            if (this.FruitingBlock == null)
            {
                await HandleFruitingBlockScan(e.Barcode);
            }

            else if (this.Shelf == null)
            {
                await HandleShelfScan(e.Barcode);
            }
        }

        private async Task HandleFruitingBlockScan(string barcode)
        {
            if (!barcode.StartsWith("FB") || barcode.Length < 3)
            {
                this.dialogBox.Show("Invalid barcode, please scan a fruiting block.");
                return;
            }

            int fruitingBlockId;
            if (!int.TryParse(barcode.Substring(2), out fruitingBlockId))
            {
                this.dialogBox.Show("Invalid barcode");
                return;
            }

            var fruitingBlock = await this.fruitingBlockApiClient.Get(fruitingBlockId);
            if (fruitingBlock == null)
            {
                this.dialogBox.Show("Fruiting block not found");
                return;
            }

            if (!fruitingBlock.GrainSpawnId.HasValue)
            {
                this.dialogBox.Show("This fruiting block has not been innoculated");
                return;
            }

            if (fruitingBlock.IncubateShelfId.HasValue)
            {
                this.dialogBox.Show("Fruiting block has already been placed on an incubation shelf, you need to place it on a fruiting shelf next");
                return;
            }

            this.FruitingBlock = fruitingBlock;
            this.ActionMessage = "Block is " + fruitingBlock.RecipeName;
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
            this.ActionMessage = "Block is " + this.FruitingBlock.RecipeName;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Shelf is " + this.Shelf.Name;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Press Confirm or Cancel to continue";
        }
    }
}