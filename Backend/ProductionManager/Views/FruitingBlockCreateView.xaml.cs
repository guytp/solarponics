using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class FruitingBlockCreateView : IFruitingBlockCreateView
    {
        public FruitingBlockCreateView(IFruitingBlockCreateViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IFruitingBlockCreateViewModel FruitingBlockCreateViewModel => DataContext as IFruitingBlockCreateViewModel;
    }
}