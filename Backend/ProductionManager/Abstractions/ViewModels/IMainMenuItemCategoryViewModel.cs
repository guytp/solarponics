using Solarponics.ProductionManager.Data;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IMainMenuItemCategoryViewModel : IViewModel
    {
        CategoryUiItem[] Categories { get; }
        
        ICommand DisplayMenuItemCommand { get; }

        ICommand SelectCategoryCommand { get; }

        MenuItem[] MenuItems { get; }
    }
}