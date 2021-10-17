using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupShelvesViewModel : IViewModel
    {
        bool IsUiEnabled { get; }
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
        bool IsAddEnabled { get; }
        bool IsShelfSelected { get; }
        Shelf[] Shelves { get; }
        Shelf SelectedShelf { get; set; }
        ICommand AddCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand PrintLabelCommand { get; }
        Location[] Locations { get; }
        Location SelectedLocation { get; set; }
        Room[] Rooms { get; }
        Room SelectedRoom { get; set; }
        public string Name { get; set; }
    }
}