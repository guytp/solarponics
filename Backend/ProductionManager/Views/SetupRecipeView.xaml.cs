using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class SetupRecipeView : ISetupRecipeView
    {
        public SetupRecipeView(ISetupRecipeViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ISetupRecipeViewModel SetupRecipeViewModel => DataContext as ISetupRecipeViewModel;
    }
}