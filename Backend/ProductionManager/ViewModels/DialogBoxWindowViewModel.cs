using System;
using System.Windows.Input;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Enums;
using Solarponics.ProductionManager.EventArgs;

namespace Solarponics.ProductionManager.ViewModels
{
    public class DialogBoxWindowViewModel : IDialogBoxWindowViewModel
    {
        public DialogBoxWindowViewModel(string title, string message, string exceptionText, string buttonText)
            : this(title, message, exceptionText, buttonText, null)
        {
        }

        public DialogBoxWindowViewModel(string title, string message, string exceptionText, string firstButtonText,
            string secondButtonText)
        {
            Title = title;
            Message = message;
            ExceptionText = exceptionText;
            FirstButtonText = firstButtonText;
            SecondButtonText = secondButtonText;
            FirstButtonCommand = new RelayCommand(_ => FirstButton());
            SecondButtonCommand = new RelayCommand(_ => SecondButton());
        }

        public string Title { get; }
        public string Message { get; }
        public string ExceptionText { get; }
        public string FirstButtonText { get; }
        public string SecondButtonText { get; }
        public bool IsSingleButton => string.IsNullOrEmpty(SecondButtonText);
        public bool IsDualButtons => !IsSingleButton;
        public ICommand FirstButtonCommand { get; }
        public ICommand SecondButtonCommand { get; }
        public bool IsTitleVisible => !string.IsNullOrEmpty(Title);
        public bool IsExceptionTextVisible => !string.IsNullOrEmpty(ExceptionText);
        public event EventHandler<DialogBoxClosedEventArgs> ButtonPressed;

        private void FirstButton()
        {
            ButtonPressed?.Invoke(this, new DialogBoxClosedEventArgs(DialogBoxButton.First));
        }

        private void SecondButton()
        {
            ButtonPressed?.Invoke(this, new DialogBoxClosedEventArgs(DialogBoxButton.Second));
        }
    }
}