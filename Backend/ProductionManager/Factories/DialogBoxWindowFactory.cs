using Solarponics.ProductionManager.Abstractions.Factories;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;
using Solarponics.ProductionManager.Views;

namespace Solarponics.ProductionManager.Factories
{
    public class DialogBoxWindowFactory : IDialogBoxWindowFactory
    {
        public IDialogBoxWindow Create(IDialogBoxWindowViewModel viewModel)
        {
            return new DialogBoxWindow(viewModel);
        }
    }
}