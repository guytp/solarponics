using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IMainMenuItemCategoryView : IView
    {
        IMainMenuItemCategoryViewModel MainMenuItemCategoryViewModel { get; }
    }
}