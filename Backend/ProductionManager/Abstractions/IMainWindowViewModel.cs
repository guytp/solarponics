namespace Solarponics.ProductionManager.Abstractions
{
    public interface IMainWindowViewModel : IViewModel
    {
        IView ActiveView { get; }
        IStatusBarViewModel StatusBarViewModel { get; }
    }
}