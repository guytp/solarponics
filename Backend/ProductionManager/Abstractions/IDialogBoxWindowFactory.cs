using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IDialogBoxWindowFactory
    {
        IDialogBoxWindow Create(IDialogBoxWindowViewModel viewModel);
    }
}