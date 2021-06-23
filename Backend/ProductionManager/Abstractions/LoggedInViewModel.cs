using System.Windows.Input;
using Solarponics.Models;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface ILoggedInViewModel : IViewModel
    {
        User User { get; set; }

        ICommand LogoutCommand { get; }
    }
}