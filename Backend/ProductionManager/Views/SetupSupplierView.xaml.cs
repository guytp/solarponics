using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class SetupSupplierView : ISetupSupplierView
    {
        public SetupSupplierView(ISetupSupplierViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ISetupSupplierViewModel SetupSupplierViewModel => DataContext as ISetupSupplierViewModel;
    }
}