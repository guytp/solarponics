using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Views
{
    public partial class StatusBarView : IStatusBarView
    {
        public StatusBarView()
        {
            InitializeComponent();
        }

        public IStatusBarViewModel StatusBarViewModel => (IStatusBarViewModel) DataContext;
    }
}