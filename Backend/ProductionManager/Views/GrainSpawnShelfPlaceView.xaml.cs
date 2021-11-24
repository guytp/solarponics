using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class GrainSpawnShelfPlaceView : IGrainSpawnShelfPlaceView
    {
        public GrainSpawnShelfPlaceView(IGrainSpawnShelfPlaceViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IGrainSpawnShelfPlaceViewModel GrainSpawnShelfPlaceViewModel => DataContext as IGrainSpawnShelfPlaceViewModel;
    }
}