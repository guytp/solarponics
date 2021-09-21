using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupSupplierViewModel : IViewModel
    {
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
        bool IsAddEnabled { get; }
        bool IsDeleteEnabled { get; }
        bool IsUiEnabled { get; }
        Supplier[] Suppliers { get; }
        Supplier SelectedSupplier { get; set; }
        ICommand AddCommand { get; }
        ICommand DeleteCommand { get; }
        string NewName { get; set; }
    }
}