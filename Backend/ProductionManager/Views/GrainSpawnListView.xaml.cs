using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class GrainSpawnListView : IGrainSpawnListView
    {
        public GrainSpawnListView(IGrainSpawnListViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IGrainSpawnListViewModel GrainSpawnListViewModel => DataContext as IGrainSpawnListViewModel;
    }
}