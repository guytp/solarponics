﻿using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IGrainSpawnShelfPlaceViewModel : IViewModel
    {
        bool IsUiEnabled { get; }

        bool IsCancelEnabled { get; }
        bool IsConfirmEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        string Notes { get; set; }

        string ActionMessage { get; }

        ICommand CancelCommand { get; }
        ICommand ConfirmCommand { get; }
    }
}