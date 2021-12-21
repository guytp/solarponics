using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class FruitingBlockFruitingShelfPlaceView : IFruitingBlockFruitingShelfPlaceView
    {
        public FruitingBlockFruitingShelfPlaceView(IFruitingBlockFruitingShelfPlaceViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IFruitingBlockFruitingShelfPlaceViewModel FruitingBlockFruitingShelfPlaceViewModel => DataContext as IFruitingBlockFruitingShelfPlaceViewModel;
    }
}