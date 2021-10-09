using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class CultureAgarLiquidPrepView : ICultureAgarLiquidPrepView
    {
        public CultureAgarLiquidPrepView(ICultureAgarLiquidPrepViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ICultureAgarLiquidPrepViewModel CultureAgarLiquidPrepViewModel => DataContext as ICultureAgarLiquidPrepViewModel;
    }
}