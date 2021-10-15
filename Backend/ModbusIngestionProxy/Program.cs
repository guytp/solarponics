using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using Solarponics.Data;

namespace Solarponics.ModbusIngestionProxy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                    .AddSingleton<ISensorModuleProvider, SensorModuleProvider>()
                    .AddSingleton<IModbusSensorCommunicatorFactory, ModbusSensorCommunicatorFactory>()
                    .AddTransient<ISensorRepository, SensorRepository>()
                    .AddHostedService<ModbusIngestionWorker>()
                    .AddTransient<IDatabaseConnection, DatabaseConnection>()
                    .AddSingleton<IStoredProcedureFactory, DapperStoredProcedureFactory>()
                    .AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>()
                    .AddSingleton<IIngestionClientFactory, IngestionClientFactory>()
                    .AddSingleton<ICommandClientFactory, CommandClientFactory>()
                    .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "Solarponics";
                            config.SourceName = "ModbusIngestionProxy";
                        });
                })
                .UseWindowsService()
                .UseSystemd()
                .Build()
                .Run();
        }
    }
}