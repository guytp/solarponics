using Solarponics.Models;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISetupRecipeViewModel : IViewModel
    {
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
        bool IsAddEnabled { get; }
        bool IsDeleteEnabled { get; }
        bool IsUiEnabled { get; }
        Recipe[] Recipes{ get; }
        Recipe SelectedRecipe { get; set; }
        ICommand AddCommand { get; }
        ICommand DeleteCommand { get; }
        string NewName { get; set; }
        string NewText { get; set; }
        string NewUnitsCreated { get; set; }
        RecipeType? NewType { get; set; }
        RecipeType?[] Types { get; }
    }
}