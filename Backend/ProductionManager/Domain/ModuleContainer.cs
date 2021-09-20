using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Data;
using System.Collections.Generic;
using System.Linq;

namespace Solarponics.ProductionManager.Domain
{
    public class ModuleContainer : IModuleContainer 
    {
        private Dictionary<string, MenuItem[]> menuItemsKeyedByCategory;

        public ModuleContainer(IModuleProvider moduleProvider)
        {
            var modules = moduleProvider.Modules;
            this.Categories = modules.OrderBy(m => m.Order).ThenBy(m => m.Category).Select(m => m.Category).Distinct().ToArray();
            this.menuItemsKeyedByCategory = new Dictionary<string, MenuItem[]>();
            foreach (var category in this.Categories)
            {
                this.menuItemsKeyedByCategory.Add(category, modules.Where(m => m.Category == category && m.MenuItems != null).SelectMany(m => m.MenuItems).OrderBy(mi => mi.Order).ThenBy(mi => mi.Name).ToArray());
            }
        }

        public string[] Categories { get; }

        public MenuItem[] GetMenuItemsForCategory(string category)
        {
            if (!this.menuItemsKeyedByCategory.ContainsKey(category))
                return null;
            return this.menuItemsKeyedByCategory[category];
        }
    }
}