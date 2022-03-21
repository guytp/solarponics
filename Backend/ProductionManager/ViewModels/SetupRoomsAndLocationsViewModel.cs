using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Data;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Abstractions.ApiClients;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupRoomsAndLocationsViewModel : ViewModelBase, ISetupRoomsAndLocationsViewModel
    {
        private readonly ILocationApiClient locationApiClient;
        private readonly IDialogBox dialogBox;
        private readonly IHardwareProvider hardwareProvider;
        private readonly IBannerNotifier bannerNotifier;

        public SetupRoomsAndLocationsViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel, ILocationApiClient locationApiClient, IDialogBox dialogBox, IHardwareProvider hardwareProvider, IBannerNotifier bannerNotifier)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.locationApiClient = locationApiClient;
            this.dialogBox = dialogBox;
            this.hardwareProvider = hardwareProvider;
            this.bannerNotifier = bannerNotifier;

            this.NewRoomCommand = new RelayCommand(_ => this.NewRoom());
            this.NewLocationCommand = new RelayCommand(_ => this.NewLocation());
            this.PrintRoomLabelCommand = new RelayCommand(_ => this.PrintRoomLabel());
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public Location[] Locations { get; private set; }

        public Location SelectedLocation { get; set; }

        public Room SelectedRoom { get; set; }

        public string NewRoomName { get; set; }

        public ICommand NewRoomCommand { get; }

        public string NewLocationName { get; set; }

        public ICommand NewLocationCommand { get; }

        public ICommand PrintRoomLabelCommand { get; }

        public bool IsUiEnabled { get; private set; }

        public bool IsNewRoomEnabled => SelectedLocation != null && !string.IsNullOrWhiteSpace(NewRoomName);
        public bool IsNewLocationEnabled => !string.IsNullOrWhiteSpace(NewLocationName);
        public bool IsPrintRoomLabelEnabled => SelectedRoom != null;
        public bool IsLocationSelected => this.SelectedLocation != null;
        public bool IsRoomSelected => this.SelectedRoom != null;


        private async void NewRoom()
        {
            if (!IsUiEnabled || !IsNewRoomEnabled)
                return;

            try
            {
                this.IsUiEnabled = false;
                var room = new Room
                {
                    LocationId = this.SelectedLocation.Id,
                    Name = this.NewRoomName
                };
                room.Id = await this.locationApiClient.AddRoom(room);
                var newRooms = new List<Room>(this.SelectedLocation.Rooms)
                {
                    room
                };
                this.SelectedLocation.Rooms = newRooms.OrderBy(o => o.Name).ToArray();
                this.OnPropertyChanged(nameof(this.SelectedLocation));
                this.NewRoomName = null;
                if (this.hardwareProvider.LabelPrinterLarge != null)
                    try
                    {
                        this.hardwareProvider.LabelPrinterLarge.Print(new RoomLabelDefinition(room));
                    }
                    catch (Exception ex2)
                    {
                        this.dialogBox.Show($"Added '{room.Name}' to '{this.SelectedLocation.Name}' but failed to print label.", exception: ex2);
                            return;
                    }
                this.bannerNotifier.DisplayMessage($"Added '{room.Name}' to '{this.SelectedLocation.Name}'");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was a problem adding the room.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private async void NewLocation()
        {
            if (!IsUiEnabled || !IsNewLocationEnabled)
                return;

            try
            {
                this.IsUiEnabled = false;
                var location = new Location
                {
                    Name = NewLocationName,
                    Rooms = new Room[0]
                };
                location.Id = await this.locationApiClient.Add(location);
                var locations = new List<Location>(this.Locations)
                {
                    location
                };
                this.Locations = locations.OrderBy(l => l.Name).ToArray();
                this.NewLocationName = null;
                this.bannerNotifier.DisplayMessage($"Added '{location.Name}'");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was a problem adding the location.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void PrintRoomLabel()
        {
            if (!IsUiEnabled || !IsPrintRoomLabelEnabled)
                return;

            if (this.hardwareProvider?.LabelPrinterLarge == null)
            {
                this.dialogBox.Show("There is no label printer configured.");
                return;
            }

            try
            {
                this.IsUiEnabled = false;
                this.hardwareProvider.LabelPrinterLarge.Print(new RoomLabelDefinition(this.SelectedRoom));
                this.bannerNotifier.DisplayMessage("Room label printed.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was a problem printing the label.", exception: ex);
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
                this.SelectedLocation = null;
                this.Locations = await this.locationApiClient.Get();
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("There was a problem loading location and room information.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }
    }
}