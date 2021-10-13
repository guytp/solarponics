using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupRoomsAndLocationsViewModel : IViewModel
    {
        Location[] Locations { get; }

        Location SelectedLocation { get; set; }

        Room SelectedRoom { get; set; }

        string NewRoomName { get; set; }

        ICommand NewRoomCommand { get; }

        string NewLocationName { get; set; }
        ICommand NewLocationCommand { get; }

        ICommand PrintRoomLabelCommand { get; }

        bool IsUiEnabled { get; }

        bool IsNewRoomEnabled { get; }
        bool IsNewLocationEnabled { get; }
        bool IsPrintRoomLabelEnabled { get; }
        bool IsRoomSelected { get; }
        bool IsLocationSelected { get; }
    }
}