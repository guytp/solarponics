using Serilog;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductionManager.Core.Abstractions;

namespace Solarponics.ProductionManager.ViewModels
{
    public class FruitingBlockListViewModel : ViewModelBase, IFruitingBlockListViewModel
    {
        private readonly IFruitingBlockApiClient apiClient;
        private readonly IHardwareProvider hardwareProvider;
        private readonly IDialogBox dialogBox;
        private readonly IBannerNotifier bannerNotifier;

        public FruitingBlockListViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, IFruitingBlockApiClient apiClient, IHardwareProvider hardwareProvider, IDialogBox dialogBox, IBannerNotifier bannerNotifier)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.apiClient = apiClient;
            this.bannerNotifier = bannerNotifier;
            this.hardwareProvider = hardwareProvider;
            this.dialogBox = dialogBox;
            this.PrintLabelCommand = new RelayCommand(o => this.PrintLabel((FruitingBlock)o));
        }

        public bool IsUiEnabled { get; private set; }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public FruitingBlock[] FruitingBlocks { get; private set; }

        public ICommand PrintLabelCommand { get; }

        private void PrintLabel(FruitingBlock fruitingBlock)
        {
            if (this.hardwareProvider.LabelPrinterLarge == null)
            {
                this.dialogBox.Show("No large label printer detected");
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                var definition = new FruitingBlockLabelDefinition(fruitingBlock);
                this.hardwareProvider.LabelPrinterLarge.Print(definition);
                this.bannerNotifier.DisplayMessage("Printed label");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to re-print fruiting block label");
                this.dialogBox.Show("Failed to print label", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        public async override Task OnShow()
        {
            try
            {
                this.IsUiEnabled = false;
                this.FruitingBlocks = await this.apiClient.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load fruiting blocks");
                this.dialogBox.Show("Failed to load fruiting block list", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
    }
}