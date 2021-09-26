using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class LoggedInButtonsView : ILoggedInButtonsView
    {
        public LoggedInButtonsView()
        {
            InitializeComponent();
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel => this.DataContext as ILoggedInButtonsViewModel;
    }
}