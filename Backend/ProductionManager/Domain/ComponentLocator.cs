using Autofac;
using Solarponics.ProductionManager.Abstractions;

namespace Solarponics.ProductionManager.Domain
{
    public class ComponentLocator : IComponentLocator
    {
        private readonly ILifetimeScope container;

        public ComponentLocator(ILifetimeScope container)
        {
            this.container = container;
        }

        public T ResolveComponent<T>()
        {
            return container.Resolve<T>();
        }
    }
}