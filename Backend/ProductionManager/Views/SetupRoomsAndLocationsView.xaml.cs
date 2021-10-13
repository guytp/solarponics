using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class SetupRoomsAndLocationsView : ISetupRoomsAndLocationsView
    {
        public SetupRoomsAndLocationsView(ISetupRoomsAndLocationsViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ISetupRoomsAndLocationsViewModel SetupRoomsAndLocationsViewModel => DataContext as ISetupRoomsAndLocationsViewModel;
    }
}