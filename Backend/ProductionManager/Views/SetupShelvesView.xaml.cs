using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class SetupShelvesView : ISetupShelvesView
    {
        public SetupShelvesView(ISetupShelvesViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ISetupShelvesViewModel SetupShelvesViewModel => DataContext as ISetupShelvesViewModel;
    }
}