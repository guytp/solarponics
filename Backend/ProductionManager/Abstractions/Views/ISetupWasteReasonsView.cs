﻿using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ISetupWasteReasonsView : IView
    {
        ISetupWasteReasonsViewModel SetupWasteReasonsViewModel { get; }
    }
}