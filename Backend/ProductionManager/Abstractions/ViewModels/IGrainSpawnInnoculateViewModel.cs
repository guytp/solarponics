using System;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IGrainSpawnInnoculateViewModel : IViewModel
    {
        bool IsUiEnabled { get; }

        bool IsCancelEnabled { get; }
        bool IsConfirmEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        string Notes { get; set; }

        string ActionMessage { get; }

        DateTime Date { get; }
        ICommand CancelCommand { get; }
        ICommand ConfirmCommand { get; }
    }
}