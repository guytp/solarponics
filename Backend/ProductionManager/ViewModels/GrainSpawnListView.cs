using Serilog;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class GrainSpawnListViewModel : ViewModelBase, IGrainSpawnListViewModel
    {
        private readonly IGrainSpawnApiClient apiClient;
        private readonly IHardwareProvider hardwareProvider;
        private readonly IDialogBox dialogBox;

        public GrainSpawnListViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, IGrainSpawnApiClient apiClient, IHardwareProvider hardwareProvider, IDialogBox dialogBox)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.apiClient = apiClient;
            this.hardwareProvider = hardwareProvider;
            this.dialogBox = dialogBox;
            this.PrintLabelCommand = new RelayCommand(o => this.PrintLabel((GrainSpawn)o));
        }

        public bool IsUiEnabled { get; private set; }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public GrainSpawn[] GrainSpawns { get; private set; }

        public ICommand PrintLabelCommand { get; }

        private void PrintLabel(GrainSpawn grainSpawn)
        {
            if (this.hardwareProvider.LabelPrinterLarge == null)
            {
                this.dialogBox.Show("No large label printer detected");
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                var definition = new GrainSpawnLabelDefinition(grainSpawn);
                this.hardwareProvider.LabelPrinterLarge.Print(definition);
                this.dialogBox.Show("Printed label");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to re-print grain spawn label");
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
                this.GrainSpawns = await this.apiClient.GetAll();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load grain spawn");
                this.dialogBox.Show("Failed to load grain spawn list", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
    }
}