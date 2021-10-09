using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Modules;

namespace Solarponics.ProductionManager.Domain
{
    public class ModuleProvider : IModuleProvider
    {
        public ModuleProvider(ISetupModule setupModule, ICultureModule cultureModule, IEnvironmentModule environmentModule)
        {
            this.Modules = new IModule[]
            {
                setupModule,
                cultureModule,
                environmentModule
            };
        }

        public IModule[] Modules { get; }
    }
}