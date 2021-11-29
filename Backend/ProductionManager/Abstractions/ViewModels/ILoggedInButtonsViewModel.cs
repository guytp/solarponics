using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ILoggedInButtonsViewModel : IViewModel
    {
        ICommand HomeCommand { get; }

        IView HomeView { get; set; }

        bool IsHomeVisible { get; }

        ICommand LogoutCommand { get; }

        ICommand ExitCommand { get; }
    }
}