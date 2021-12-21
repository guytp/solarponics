using Solarponics.Models;
using System;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IFruitingBlockCreateViewModel : IViewModel
    {
        bool IsAddEnabled { get; }

        bool IsUiEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        Recipe SelectedRecipe { get; set; }

        Recipe[] Recipes { get; }

        string Weight { get; set; }

        string Notes { get; set; }
        string Quantity { get; set; }

        ICommand AddCommand { get; }
        DateTime Date { get; }
    }
}