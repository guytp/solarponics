using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupAutoclavesViewModel : IViewModel
    {
        bool IsUiEnabled { get; }
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
        bool IsAddEnabled { get; }
        bool IsAutoclaveSelected { get; }
        Autoclave[] Autoclaves { get; }
        Autoclave SelectedAutoclave { get; set; }
        ICommand AddCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand PrintLabelCommand { get; }
        Location[] Locations { get; }
        Location SelectedLocation { get; set; }
        Room[] Rooms { get; }
        Room SelectedRoom { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
    }
}