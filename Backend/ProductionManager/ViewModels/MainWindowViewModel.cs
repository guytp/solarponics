using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Core;

namespace Solarponics.ProductionManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
    {
        private IView activeView;

        public MainWindowViewModel(INavigator navigator, IStatusBarViewModel statusBarViewModel)
        {
            this.StatusBarViewModel = statusBarViewModel;
            ActiveView = navigator.ActiveView;
            navigator.ViewChanged += (sender, args) => ActiveView = navigator.ActiveView;
        }

        public IStatusBarViewModel StatusBarViewModel { get; }

        public IView ActiveView
        {
            get => activeView;
            private set => activeView = value;
        }
    }
}