using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.ViewModels;

namespace Solarponics.ProductionManager.Factories
{
    public class DialogBoxWindowViewModelFactory : IDialogBoxWindowViewModelFactory
    {
        public IDialogBoxWindowViewModel Create(string title, string message, string exceptionText, string buttonText)
        {
            return new DialogBoxWindowViewModel(title, message, exceptionText, buttonText);
        }

        public IDialogBoxWindowViewModel Create(string title, string message, string exceptionText,
            string firstButtonText,
            string secondButtonText)
        {
            return new DialogBoxWindowViewModel(title, message, exceptionText, firstButtonText, secondButtonText);
        }
    }
}