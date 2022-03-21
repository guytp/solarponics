using System.Timers;
using ProductionManager.Core.Abstractions;
using ProductionManager.Core.Data;
using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.ViewModels
{
    public class BannerNotificationViewModel : ViewModelBase, IBannerNotificationViewModel
    {
        private readonly Timer timer;
        public BannerNotificationViewModel(IBannerNotifier bannerNotifier)
        {
            bannerNotifier.BannerRequested += OnBannerRequested;
            this.timer = new Timer
            {
                AutoReset = false,
                Enabled = false,
                Interval = 5000
            };
            this.timer.Elapsed += OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.IsMessageVisible = false;
        }

        private void OnBannerRequested(object sender, BannerNotificationEventArgs e)
        {
            this.timer.Enabled = false;
            this.Message = e.Message;
            this.IsMessageVisible = !string.IsNullOrEmpty(e.Message);
            this.timer.Enabled = true;
        }

        public string Message { get; private set; }

        public bool IsMessageVisible { get; private set; }
    }
}