using System.Windows.Input;

namespace Solarponics.ProductionManagerMobile.Abstractions.ViewModels
{
    public interface ILoginViewModel
    {
        bool IsEnabled { get; }
        bool IsLoginEnabled { get; }
        string UserId { get; set; }
        string Pin { get; set; }
        ICommand LoginCommand { get; }
    }
}