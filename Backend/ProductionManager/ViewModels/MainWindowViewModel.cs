using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        private IView activeView;

        public MainWindowViewModel(INavigator navigator, IStatusBarViewModel statusBarViewModel, IBannerNotificationViewModel bannerNotificationViewModel)
        {
            this.StatusBarViewModel = statusBarViewModel;
            this.BannerNotificationViewModel = bannerNotificationViewModel;
            ActiveView = navigator.ActiveView;
            navigator.ViewChanged += (sender, args) => ActiveView = navigator.ActiveView;
        }

        public IBannerNotificationViewModel BannerNotificationViewModel { get; }

        public IStatusBarViewModel StatusBarViewModel { get; }

        public IView ActiveView
        {
            get => activeView;
            private set => activeView = value;
        }
    }
}