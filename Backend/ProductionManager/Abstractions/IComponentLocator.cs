namespace Solarponics.ProductionManager.Abstractions
{
    public interface IComponentLocator
    {
        T ResolveComponent<T>();
    }
}