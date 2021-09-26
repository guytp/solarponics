using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Abstractions.Factories
{
    public interface IDialogBoxWindowFactory
    {
        IDialogBoxWindow Create(IDialogBoxWindowViewModel viewModel);
    }
}