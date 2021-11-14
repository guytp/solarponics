using Solarponics.Models;
using Solarponics.ProductionManager.Data;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ICultureListViewModel : IViewModel
    {
        bool IsUiEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        CultureDataGridRowEntry[] Cultures { get; }

        ICommand PrintLabelCommand { get; }
    }
}