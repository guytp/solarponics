using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupWasteReasonsViewModel : ViewModelBase, ISetupWasteReasonsViewModel
    {
        private readonly IWasteReasonApiClient apiClient;
        private readonly IDialogBox dialogBox;

        public SetupWasteReasonsViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, IWasteReasonApiClient apiClient, IDialogBox dialogBox, IHardwareProvider hardwareProvider)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.apiClient = apiClient;
            this.dialogBox = dialogBox;
            this.AddCommand = new RelayCommand(_ => this.Add());
            this.DeleteCommand = new RelayCommand(_ => this.Delete());
            this.IsUiEnabled = true;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public bool IsAddEnabled => !string.IsNullOrWhiteSpace(this.Reason);
        public bool IsWasteReasonSelected => this.SelectedWasteReason != null;
        public bool IsUiEnabled { get; private set; }
        public WasteReason[] WasteReasons { get; private set; }
        public WasteReason SelectedWasteReason { get; set; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public string Reason { get; set; }

        public override async Task OnShow()
        {
            try
            {
                this.IsUiEnabled = false;
                this.ResetUi();
                this.WasteReasons = await this.apiClient.Get();
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Error loading waste reason details", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
        
        private void ResetUi()
        {
            this.SelectedWasteReason = null;
            this.Reason = null;
        }

        private async void Add()
        {
            if (!this.IsUiEnabled || !this.IsAddEnabled)
                return;

            try
            {
                this.IsUiEnabled = false;
                var id = await this.apiClient.Add(new WasteReason
                {
                    Reason = Reason
                });
                this.ResetUi();
                this.WasteReasons = await this.apiClient.Get();
                this.dialogBox.Show("Waste reason added.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to add waste reason.", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private async void Delete()
        {
            if (!this.IsUiEnabled || !this.IsWasteReasonSelected || this.SelectedWasteReason == null)
                return;

            if (!this.dialogBox.Show("Do you want to delete " + this.SelectedWasteReason.Reason + "?", buttons: Enums.DialogBoxButtons.YesNo))
            {
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                await this.apiClient.Delete(this.SelectedWasteReason.Id);
                this.WasteReasons = this.WasteReasons.Where(s => s != this.SelectedWasteReason).ToArray();
                this.SelectedWasteReason = null;
                this.dialogBox.Show("Waste reason deleted.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to delete recipe.", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
    }
}