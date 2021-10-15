using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solarponics.ModbusIngestionProxy
{
    public class SensorModuleProvider : ISensorModuleProvider
    {
        private DateTime nextRead = DateTime.UtcNow;

        private List<IModbusSensorCommunicator> communicators = new List<IModbusSensorCommunicator>();
        private readonly IModbusSensorCommunicatorFactory factory;
        private readonly ISensorRepository repo;
        public SensorModuleProvider(IModbusSensorCommunicatorFactory factory, ISensorRepository repo)
        {
            this.factory = factory;
            this.repo = repo;
        }

        public async Task<IModbusSensorCommunicator[]> GetModulesRequiringRead(int maximumCount, IModbusSensorCommunicator[] excludeCommunicators)
        {
            if (DateTime.UtcNow >= nextRead)
                await this.RefreshSensorModules();

            return this.communicators.Where(sm => (!sm.LastReadingsSubmitted.HasValue || DateTime.UtcNow.Subtract(sm.LastReadingsSubmitted.Value).TotalSeconds > 15) && (!(excludeCommunicators ?? new IModbusSensorCommunicator[0]).Any(c => c == sm)))
                .OrderBy(sm => sm.LastReadingsSubmitted ?? new DateTime(0))
                .Take(maximumCount)
                .ToArray();
        }

        private async Task RefreshSensorModules()
        {
            
            Console.WriteLine("Updating list of sensor modules");
            this.nextRead = DateTime.UtcNow.AddSeconds(10);

            var modules = await this.repo.GetSensorModules();

            // Add new and do nothing to unchanged
            var toRemove = new List<IModbusSensorCommunicator>();
            foreach (var module in modules)
            {
                var communicator = communicators.FirstOrDefault(m => m.SensorModule.Id == module.Id);

                if (communicator == null)
                {
                    Console.WriteLine($"Sensor module {module.Name} / {module.Id} is new, adding");
                    this.communicators.Add(this.factory.Create(module));
                    continue;
                }

                if (module == communicator.SensorModule)
                    continue;


                Console.WriteLine($"Sensor module {communicator.SensorModule.Name} / {communicator.SensorModule.Id} is updating, flushing");
                toRemove.Add(communicator);
                this.communicators.Add(this.factory.Create(module));
            }

            // Remove any no longer on list
            foreach (var existing in this.communicators)
            {
                if (!modules.Any(m => m.Id == existing.SensorModule.Id))
                {
                    Console.WriteLine($"Sensor module {existing.SensorModule.Name} / {existing.SensorModule.Id} is no longer present, removing");
                    toRemove.Add(existing);
                }
            }
            foreach (var remove in toRemove)
            {
                (remove as IDisposable)?.Dispose();
                this.communicators.Remove(remove);
            }
        }
    }
}
