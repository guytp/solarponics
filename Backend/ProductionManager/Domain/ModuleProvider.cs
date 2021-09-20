using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Modules;

namespace Solarponics.ProductionManager.Domain
{
    public class ModuleProvider : IModuleProvider
    {
        public ModuleProvider(ISetupModule setupModule, ICultureModule cultureModule)
        {
            this.Modules = new IModule[]
            {
                setupModule,
                cultureModule
            };
        }

        public IModule[] Modules { get; }
    }
}