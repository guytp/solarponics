using Solarponics.ProductionManager.Abstractions;

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