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
using Solarponics.ProductionManager.Core.Enums;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupAutoclavesViewModel : ViewModelBase, ISetupAutoclavesViewModel
    {
        private readonly IAutoclaveApiClient apiClient;
        private readonly IDialogBox dialogBox;
        private readonly IHardwareProvider hardwareProvider;
        private readonly ILocationApiClient locationApiClient;
        private readonly IBannerNotifier bannerNotifier;

        public SetupAutoclavesViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, IAutoclaveApiClient apiClient, IDialogBox dialogBox, IHardwareProvider hardwareProvider, ILocationApiClient locationApiClient, IBannerNotifier bannerNotifier)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.apiClient = apiClient;
            this.dialogBox = dialogBox;
            this.bannerNotifier = bannerNotifier;
            this.locationApiClient = locationApiClient;
            this.AddCommand = new RelayCommand(_ => this.Add());
            this.PrintLabelCommand = new RelayCommand(_ => this.PrintLabel());
            this.DeleteCommand = new RelayCommand(_ => this.Delete());
            this.IsUiEnabled = true;
            this.hardwareProvider = hardwareProvider;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public bool IsAddEnabled => !string.IsNullOrWhiteSpace(this.Name) && SelectedRoom != null;
        public bool IsAutoclaveSelected => this.SelectedAutoclave != null;
        public bool IsUiEnabled { get; private set; }
        public Autoclave[] Autoclaves { get; private set; }
        public Autoclave SelectedAutoclave { get; set; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand PrintLabelCommand { get; }
        public Room[] Rooms { get; private set; }
        public Location[] Locations { get; private set; }
        public Room SelectedRoom { get; set; }
        public Location SelectedLocation { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }

        public override async Task OnShow()
        {
            try
            {
                this.IsUiEnabled = false;
                this.ResetUi();
                this.Autoclaves = await this.apiClient.Get();
                this.Locations = await this.locationApiClient.Get();
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Error loading autoclave details", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
        
        private void ResetUi()
        {
            this.SelectedAutoclave = null;
            this.Name = null;
            this.Details = null;
            this.SelectedRoom = null;
            this.SelectedLocation = null;
        }

        private async void Add()
        {
            if (!this.IsUiEnabled || !this.IsAddEnabled)
                return;

            try
            {
                this.IsUiEnabled = false;
                var id = await this.apiClient.Add(new Autoclave
                {
                    Name = Name,
                    RoomId = SelectedRoom.Id,
                    Details = Details,
                });
                this.ResetUi();
                this.Autoclaves = await this.apiClient.Get();
                if (this.hardwareProvider?.LabelPrinterLarge != null)
                {
                    try
                    {
                        this.hardwareProvider.LabelPrinterLarge.Print(new AutoclaveLabelDefinition(this.Autoclaves.First(ac => ac.Id == id)));
                    }
                    catch (Exception ex)
                    {
                        this.dialogBox.Show("Failed to print label.", exception: ex);
                    }
                }
                this.bannerNotifier.DisplayMessage("Autoclave added.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to add autoclave.", ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void PrintLabel()
        {
            if (!IsUiEnabled || !IsAutoclaveSelected)
                return;

            if (hardwareProvider?.LabelPrinterLarge == null)
            {
                this.dialogBox.Show("No printer is attached to the system.");
                return;
            }

            try
            {
                hardwareProvider.LabelPrinterLarge.Print(new AutoclaveLabelDefinition(this.SelectedAutoclave));
                this.bannerNotifier.DisplayMessage("Label printed.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to print label.", exception: ex);
            }
        }

        private async void Delete()
        {
            if (!this.IsUiEnabled || !this.IsAutoclaveSelected || this.SelectedAutoclave == null)
                return;

            if (!this.dialogBox.Show("Do you want to delete " + this.SelectedAutoclave.Name + "?", buttons: DialogBoxButtons.YesNo))
            {
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                await this.apiClient.Delete(this.SelectedAutoclave.Id);
                this.Autoclaves = this.Autoclaves.Where(s => s != this.SelectedAutoclave).ToArray();
                this.SelectedAutoclave = null;
                this.bannerNotifier.DisplayMessage("Autoclave deleted.");
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

        private void OnSelectedLocationChanged()
        {
            this.Rooms = this.SelectedLocation?.Rooms;
        }
    }
}