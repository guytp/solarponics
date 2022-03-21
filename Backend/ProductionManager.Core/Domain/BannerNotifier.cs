using System;
using ProductionManager.Core.Abstractions;
using ProductionManager.Core.Data;

namespace Solarponics.ProductionManager.Core.Domain
{
    public class BannerNotifier : IBannerNotifier
    {
        public event EventHandler<BannerNotificationEventArgs> BannerRequested;

        public void DisplayMessage(string text)
        {
            BannerRequested?.Invoke(this, new BannerNotificationEventArgs(text));
        }
    }
}