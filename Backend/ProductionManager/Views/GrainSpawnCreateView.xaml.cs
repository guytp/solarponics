using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class GrainSpawnCreateView : IGrainSpawnCreateView
    {
        public GrainSpawnCreateView(IGrainSpawnCreateViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IGrainSpawnCreateViewModel GrainSpawnCreateViewModel => DataContext as IGrainSpawnCreateViewModel;
    }
}