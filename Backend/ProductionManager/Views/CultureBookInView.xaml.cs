using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class CultureBookInView : ICultureBookInView
    {
        public CultureBookInView(ICultureBookInViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ICultureBookInViewModel CultureBookInViewModel => DataContext as ICultureBookInViewModel;
    }
}