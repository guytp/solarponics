using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class GrainSpawnMixView : IGrainSpawnMixView
    {
        public GrainSpawnMixView(IGrainSpawnMixViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IGrainSpawnMixViewModel GrainSpawnMixViewModel => DataContext as IGrainSpawnMixViewModel;
    }
}