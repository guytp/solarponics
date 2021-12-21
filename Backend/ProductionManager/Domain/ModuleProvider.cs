using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Modules;

namespace Solarponics.ProductionManager.Domain
{
    public class ModuleProvider : IModuleProvider
    {
        public ModuleProvider(ISetupModule setupModule, ICultureModule cultureModule, IEnvironmentModule environmentModule, IGrainSpawnModule grainSpawnModule, IFruitingBlockModule fruitingBlockModule)
        {
            this.Modules = new IModule[]
            {
                setupModule,
                cultureModule,
                environmentModule,
                grainSpawnModule,
                fruitingBlockModule
            };
        }

        public IModule[] Modules { get; }
    }
}