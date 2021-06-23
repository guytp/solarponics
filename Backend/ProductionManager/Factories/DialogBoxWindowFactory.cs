using Solarponics.ProductionManager.Abstractions;
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