using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface ILoginViewModel : IViewModel
    {
        bool IsEnabled { get; }
        bool IsLoginEnabled { get; }
        ICommand LoginCommand { get; }
        string UserId { get; set; }

        string Pin { get; set; }
    }
}