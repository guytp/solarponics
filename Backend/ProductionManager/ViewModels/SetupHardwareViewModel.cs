using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Core;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupHardwareViewModel : ViewModelBase, ISetupHardwareViewModel
    {
        public SetupHardwareViewModel(ILoggedInButtonsViewModel loggedInButtonsViewModel)
        {
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }
    }
}