namespace Solarponics.ProductionManager.Abstractions
{
    public interface IDialogBoxWindowFactory
    {
        IDialogBoxWindow Create(IDialogBoxWindowViewModel viewModel);
    }
}