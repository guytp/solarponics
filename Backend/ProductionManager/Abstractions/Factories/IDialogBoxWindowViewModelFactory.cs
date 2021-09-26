using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Factories
{
    public interface IDialogBoxWindowViewModelFactory
    {
        IDialogBoxWindowViewModel Create(string title, string message, string exceptionText, string buttonText);

        IDialogBoxWindowViewModel Create(string title, string message, string exceptionText, string firstButtonText,
            string secondButtonText);
    }
}