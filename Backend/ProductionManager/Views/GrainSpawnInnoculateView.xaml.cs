using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class GrainSpawnInnoculateView : IGrainSpawnInnoculateView
    {
        public GrainSpawnInnoculateView(IGrainSpawnInnoculateViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IGrainSpawnInnoculateViewModel GrainSpawnInnoculateViewModel => DataContext as IGrainSpawnInnoculateViewModel;
    }
}