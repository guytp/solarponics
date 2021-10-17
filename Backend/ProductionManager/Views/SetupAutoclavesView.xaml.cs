using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class SetupAutoclavesView : ISetupAutoclavesView
    {
        public SetupAutoclavesView(ISetupAutoclavesViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ISetupAutoclavesViewModel SetupAutoclavesViewModel => DataContext as ISetupAutoclavesViewModel;
    }
}