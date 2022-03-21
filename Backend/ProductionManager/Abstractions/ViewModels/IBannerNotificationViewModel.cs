namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IBannerNotificationViewModel
    {
        string Message { get; }

        bool IsMessageVisible { get; }
    }
}