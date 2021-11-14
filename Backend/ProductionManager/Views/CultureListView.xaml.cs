using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class CultureListView : ICultureListView
    {
        public CultureListView(ICultureListViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ICultureListViewModel CultureListViewModel => DataContext as ICultureListViewModel;
    }
}