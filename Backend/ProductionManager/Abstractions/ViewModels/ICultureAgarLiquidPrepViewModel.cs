using Solarponics.Models;
using System;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ICultureAgarLiquidPrepViewModel : IViewModel
    {
        bool IsGenerateEnabled { get; }

        bool IsUiEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        Recipe[] Recipes { get; }

        string Quantity { get; set; }

        Recipe SelectedRecipe { get; set; }

        CultureMediumType? SelectedMediumType { get; set; }
        
        CultureMediumType[] MediumTypes { get; }

        string Notes { get; set; }

        ICommand GenerateCommand { get; }
        DateTime Date { get; }
    }
}