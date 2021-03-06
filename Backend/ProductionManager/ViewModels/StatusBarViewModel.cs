using System;
using System.Timers;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Core.Abstractions;

namespace Solarponics.ProductionManager.ViewModels
{
    public class StatusBarViewModel : ViewModelBase, IStatusBarViewModel
    {
        private Timer timer;
        public StatusBarViewModel(IAuthenticationSession authenticationSession)
        {
            authenticationSession.LoggedIn += (sender, args) => UserName = authenticationSession.User.Name;
            authenticationSession.LoggedOut += (sender, args) => UserName = null;
            UpdateTime();
            this.timer = new Timer(1000);
            this.timer.Elapsed += (sender, args) => this.UpdateTime();
            this.timer.Enabled = true;
        }

        public string UserName { get; private set; }
        public string Date { get; private set; }
        public string Time { get; private set; }
        public int DayOfYear { get; private set; }

        private void UpdateTime()
        {
            var time = DateTime.Now;
            this.Time = time.ToShortTimeString();
            this.Date = time.ToLongDateString();
            this.DayOfYear = time.DayOfYear;
        }
    }
}