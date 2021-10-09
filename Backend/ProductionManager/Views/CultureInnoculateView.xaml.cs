using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class CultureInnoculateView : ICultureInnoculateView
    {
        public CultureInnoculateView(ICultureInnoculateViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ICultureInnoculateViewModel CultureInnoculateViewModel => DataContext as ICultureInnoculateViewModel;
    }
}