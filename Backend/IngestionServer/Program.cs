using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using Solarponics.Data;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.Data;
using Solarponics.IngestionServer.Domain;
using Solarponics.IngestionServer.Net;

namespace Solarponics.IngestionServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IMessageHandlerSelector, MessageHandlerSelector>();
                    services.AddSingleton<INetworkServer, NetworkServer>();
                    services.AddSingleton<INetworkSessionFactory, NetworkSessionFactory>();
                    services.AddSingleton<IOpCodeToTypeConverter, OpCodeToTypeConverter>();
                    services.AddTransient<IDatabaseConnection, DatabaseConnection>();
                    services.AddSingleton<IStoredProcedureFactory, DapperStoredProcedureFactory>();
                    services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();
                    services.AddTransient<ISensorRepository, SensorRepository>();
                    services.AddHostedService<NetworkServerWorker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "Solarponics";
                            config.SourceName = "Ingestion Server";
                        });
                })
                .UseWindowsService()
                .UseSystemd();
        }
    }
}