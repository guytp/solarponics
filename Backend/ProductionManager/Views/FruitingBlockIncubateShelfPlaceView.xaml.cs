using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class FruitingBlockIncubateShelfPlaceView : IFruitingBlockIncubateShelfPlaceView
    {
        public FruitingBlockIncubateShelfPlaceView(IFruitingBlockIncubateShelfPlaceViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IFruitingBlockIncubateShelfPlaceViewModel FruitingBlockIncubateShelfPlaceViewModel => DataContext as IFruitingBlockIncubateShelfPlaceViewModel;
    }
}