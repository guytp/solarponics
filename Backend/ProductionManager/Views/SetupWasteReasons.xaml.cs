using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class SetupWasteReasonsView : ISetupWasteReasonsView
    {
        public SetupWasteReasonsView(ISetupWasteReasonsViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ISetupWasteReasonsViewModel SetupWasteReasonsViewModel => DataContext as ISetupWasteReasonsViewModel;
    }
}