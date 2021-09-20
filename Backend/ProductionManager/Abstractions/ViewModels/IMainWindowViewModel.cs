namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IMainWindowViewModel : IViewModel
    {
        IView ActiveView { get; }
        IStatusBarViewModel StatusBarViewModel { get; }
    }
}