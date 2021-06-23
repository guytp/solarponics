namespace Solarponics.ProductionManager.Abstractions
{
    public interface IDialogBoxWindowViewModelFactory
    {
        IDialogBoxWindowViewModel Create(string title, string message, string exceptionText, string buttonText);

        IDialogBoxWindowViewModel Create(string title, string message, string exceptionText, string firstButtonText,
            string secondButtonText);
    }
}