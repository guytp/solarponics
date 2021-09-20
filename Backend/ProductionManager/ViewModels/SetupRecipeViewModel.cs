using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Core;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupRecipeViewModel : ViewModelBase, ISetupRecipeViewModel
    {
        public SetupRecipeViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
    }
}