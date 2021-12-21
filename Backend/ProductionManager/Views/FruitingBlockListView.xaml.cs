using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class FruitingBlockListView : IFruitingBlockListView
    {
        public FruitingBlockListView(IFruitingBlockListViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IFruitingBlockListViewModel FruitingBlockListViewModel => DataContext as IFruitingBlockListViewModel;
    }
}