using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Solarponics.ModbusIngestionProxy
{
    public class ModbusIngestionWorker : BackgroundService
    {
        private readonly ISensorModuleProvider sensorModuleProvider;
        public ModbusIngestionWorker(ISensorModuleProvider sensorModuleProvider)
        {
            this.sensorModuleProvider = sensorModuleProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Console.WriteLine("Modbus Ingestion Worker started");

            var pendingTasks = new Dictionary<IModbusSensorCommunicator, Task<SensorModuleReading>>();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var sensorModules = (await this.sensorModuleProvider.GetModulesRequiringRead(Environment.ProcessorCount - pendingTasks.Count, pendingTasks.Keys.ToArray()));
                    if (sensorModules.Length > 0)
                        Console.WriteLine($"Adding {sensorModules.Length} sensors to queue, with {pendingTasks.Count} tasks pre-existing");

                    foreach (var kvp in pendingTasks.Where(kvp => kvp.Value.IsCompleted).ToArray())
                    {
                        var task = kvp.Value;

                        var communicator = kvp.Key;
                        pendingTasks.Remove(kvp.Key);
                        if (task.IsFaulted)
                        {
                            Console.WriteLine($"Failed to get readings from sensor module {communicator.SensorModule.Name}: " + task.Exception);
                            continue;
                        }

                        communicator.SubmitIngestionReading(task.Result);
                    }

                    await Task.Delay(100, stoppingToken);

                    foreach (var sensorModule in sensorModules)
                    {
                        var task = sensorModule.GetReadings();
                        pendingTasks.Add(sensorModule, task);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fatal error: " + ex);
                    await Task.Delay(5000);
                }

            }

            Console.WriteLine("Modbus Ingestion Worker finished");
        }
    }
}