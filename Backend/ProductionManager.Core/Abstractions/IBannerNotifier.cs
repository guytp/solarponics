using System;
using ProductionManager.Core.Data;

namespace ProductionManager.Core.Abstractions
{
    public interface IBannerNotifier
    {
        event EventHandler<BannerNotificationEventArgs> BannerRequested;

        void DisplayMessage(string text);
    }
}