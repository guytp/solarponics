using Solarponics.Models;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ILoggedInViewModel : IViewModel
    {
        User User { get; set; }
        
        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        IMainMenuItemCategoryViewModel MainMenuItemCategoryViewModel { get; }
    }
}