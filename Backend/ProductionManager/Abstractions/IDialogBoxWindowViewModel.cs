using System;
using System.Windows.Input;
using Solarponics.ProductionManager.EventArgs;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IDialogBoxWindowViewModel
    {
        string Title { get; }
        string Message { get; }
        string ExceptionText { get; }
        string FirstButtonText { get; }
        string SecondButtonText { get; }
        bool IsSingleButton { get; }
        bool IsDualButtons { get; }
        ICommand FirstButtonCommand { get; }
        ICommand SecondButtonCommand { get; }
        bool IsTitleVisible { get; }
        bool IsExceptionTextVisible { get; }
        event EventHandler<DialogBoxClosedEventArgs> ButtonPressed;
    }
}