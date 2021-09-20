using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class SetupHardwareView : ISetupHardwareView
    {
        public SetupHardwareView(ISetupHardwareViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ISetupHardwareViewModel SetupHardwareViewModel => DataContext as ISetupHardwareViewModel;
    }
}