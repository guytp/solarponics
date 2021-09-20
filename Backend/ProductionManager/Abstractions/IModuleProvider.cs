namespace Solarponics.ProductionManager.Abstractions
{
    public interface IModuleProvider
    {
        IModule[] Modules { get; }
    }
}