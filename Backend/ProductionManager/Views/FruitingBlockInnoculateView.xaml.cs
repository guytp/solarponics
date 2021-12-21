using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class FruitingBlockInnoculateView : IFruitingBlockInnoculateView
    {
        public FruitingBlockInnoculateView(IFruitingBlockInnoculateViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IFruitingBlockInnoculateViewModel FruitingBlockInnoculateViewModel => DataContext as IFruitingBlockInnoculateViewModel;
    }
}