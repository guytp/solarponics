using System;

namespace ProductionManager.Core.Data
{
    public class BannerNotificationEventArgs : EventArgs
    {
        public string Message { get; }

        public BannerNotificationEventArgs(string message)
        {
            Message = message;
        }
    }
}