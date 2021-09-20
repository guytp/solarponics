using Solarponics.ProductionManager.Abstractions;

namespace Solarponics.ProductionManager.Data
{
    public class MenuItem
    {
        public MenuItem(string name, IView view, int order = 10000000)
        {
            this.View = view;
            this.Name = name;
            this.Order = order;
        }

        public string Name { get; }

        public int Order { get; }

        public IView View { get; }
    }
}