using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ISetupRecipeView : IView
    {
        ISetupRecipeViewModel SetupRecipeViewModel { get; }
    }
}