using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class MainMenuItemCategoryView :  IMainMenuItemCategoryView
    {
        public MainMenuItemCategoryView()
        {
            InitializeComponent();
        }
        
        public IMainMenuItemCategoryViewModel MainMenuItemCategoryViewModel => this.DataContext as IMainMenuItemCategoryViewModel;
    }
}