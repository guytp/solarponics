using Solarponics.Models;
using System;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ICultureBookInViewModel : IViewModel
    {
        bool IsBookInEnabled { get; }

        bool IsUiEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        Supplier SelectedSupplier { get; set; }

        Supplier[] Suppliers { get; }

        CultureMediumType? MediumType { get; set; }
        CultureMediumType[] MediumTypes { get; set; }

        DateTime? OrderDate { get; set; }

        string Strain { get; set; }

        string Notes { get; set; }

        ICommand BookInCommand { get; }
    }
}