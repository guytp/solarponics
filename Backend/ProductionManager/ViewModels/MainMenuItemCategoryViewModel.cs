using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using Solarponics.ProductionManager.Data;
using System.Linq;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class MainMenuItemCategoryViewModel : ViewModelBase, IMainMenuItemCategoryViewModel
    {
        private readonly IModuleContainer moduleContainer;
        private readonly INavigator navigator;

        public MainMenuItemCategoryViewModel(IModuleContainer moduleContainer, INavigator navigator)
        {
            this.moduleContainer = moduleContainer;
            this.navigator = navigator;
            this.Categories = moduleContainer.Categories.Select(c => new CategoryUiItem(c)).ToArray();
            this.SelectCategoryCommand = new RelayCommand(o => this.SelectCategory(o as string));
            this.DisplayMenuItemCommand = new RelayCommand(o => this.DisplayMenuItem(o as MenuItem));
        }

        public CategoryUiItem[] Categories { get; }

        public ICommand DisplayMenuItemCommand { get; }

        public ICommand SelectCategoryCommand { get; }

        public MenuItem[] MenuItems { get; private set; }

        private void DisplayMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                return;

            this.navigator.NavigateTo(menuItem.View);
        }

        private void SelectCategory(string name)
        {
            var menuItems = new MenuItem[0];

            foreach (var category in this.Categories)
            {
                if (category.Name != name)
                {
                    category.IsSelected = false;
                    continue;
                }

                category.IsSelected = !category.IsSelected;
                if (category.IsSelected)
                {
                    menuItems = this.moduleContainer.GetMenuItemsForCategory(name);
                }
            }

            this.MenuItems = menuItems;
        }
    }
}