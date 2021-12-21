using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class FruitingBlockInnoculateViewModel : ViewModelBase, IFruitingBlockInnoculateViewModel
    {
        private readonly IDialogBox dialogBox;
        private readonly IFruitingBlockApiClient fruitingBlockApiClient;
        private readonly IHardwareProvider hardwareProvider;
        private readonly IGrainSpawnApiClient grainSpawnApiClient;

        public FruitingBlockInnoculateViewModel(IDialogBox dialogBox, IFruitingBlockApiClient fruitingBlockApiClient, IGrainSpawnApiClient grainSpawnApiClient, IHardwareProvider hardwareProvider, ILoggedInButtonsViewModel loggedInButtonsViewModel)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.dialogBox = dialogBox;
            this.fruitingBlockApiClient = fruitingBlockApiClient;
            this.grainSpawnApiClient = grainSpawnApiClient;
            this.hardwareProvider = hardwareProvider;
            this.ConfirmCommand = new RelayCommand(_ => this.Confirm());
            this.CancelCommand = new RelayCommand(_ => this.Cancel());
            this.IsUiEnabled = true;
        }

        public bool IsUiEnabled { get; private set; }

        public bool IsConfirmEnabled => FruitingBlock != null && GrainSpawn != null;
        public bool IsCancelEnabled => FruitingBlock != null || GrainSpawn != null;
        public DateTime Date { get; set; }
        private FruitingBlock FruitingBlock { get; set; }

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
                this.dialogBox.Show("Unable to innoculate fruiting block without large label printer");
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                var fruitingBlock = await this.fruitingBlockApiClient.Innoculate(this.FruitingBlock.Id, new Models.WebApi.FruitingBlockInnoculateRequest
                {
                    AdditionalNotes = Notes,
                    GrainSpawnId = this.GrainSpawn.Id,
                    Date = this.Date
                });

                this.hardwareProvider.LabelPrinterLarge.Print(new FruitingBlockLabelDefinition(fruitingBlock));
                this.ResetUi();
                this.dialogBox.Show("Confirmed innoculation and new label printed");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was an unexpected problem confirming the innoculation.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void ResetUi()
        {
            this.FruitingBlock = null;
            this.GrainSpawn = null;
            this.Notes = null;
            this.ActionMessage = "Scan the fruiting block.";
        }

        private async void OnBarcodeRead(object sender, Data.BarcodeReadEventArgs e)
        {
            if ((this.GrainSpawn != null && this.FruitingBlock != null) || !this.IsUiEnabled)
            {
                return;
            }

            if (this.FruitingBlock == null)
            {
                await HandleFruitingBlockScan(e.Barcode);
            }

            else if (this.GrainSpawn == null)
            {
                await HandleGrainSpawnScan(e.Barcode);
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

            if (fruitingBlock.GrainSpawnId.HasValue)
            {
                this.dialogBox.Show("This fruiting block has already been innoculated");
                return;
            }

            this.FruitingBlock = fruitingBlock;
            this.ActionMessage = "Block is " + fruitingBlock.RecipeName;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Please scan grain spawn";
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
            if (grainSpawn== null)
            {
                this.dialogBox.Show("Grain spawn not found");
                return;
            }

            if (!grainSpawn.InnoculateDate.HasValue)
            {
                this.dialogBox.Show("You scanned a non-innoculated grain spawn, it must be innoculated for use in fruiting block");
                return;
            }

            this.GrainSpawn = grainSpawn;
            this.ActionMessage = "Block is " + this.FruitingBlock.RecipeName;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Spawn is " + this.GrainSpawn.Strain;
            this.ActionMessage += Environment.NewLine + Environment.NewLine;
            this.ActionMessage += "Press Confirm or Cancel to continue";
        }
    }
}