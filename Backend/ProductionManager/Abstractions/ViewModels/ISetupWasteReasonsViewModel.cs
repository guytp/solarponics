using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupWasteReasonsViewModel : IViewModel
    {
        bool IsUiEnabled { get; }
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
        bool IsAddEnabled { get; }
        bool IsWasteReasonSelected { get; }
        WasteReason[] WasteReasons { get; }
        WasteReason SelectedWasteReason { get; set; }
        ICommand AddCommand { get; }
        ICommand DeleteCommand { get; }
        public string Reason { get; set; }
    }
}