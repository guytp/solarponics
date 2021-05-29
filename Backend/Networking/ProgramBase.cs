using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using Solarponics.Data;
using Solarponics.Networking.Abstractions;
using Solarponics.Networking.Domain;

namespace Solarponics.Networking
{
    public abstract class ProgramBase
    {
        protected static IHostBuilder GenerateHostBuilder<TMessageHandlerSelector, TNetworkServer, TNetworkSessionFactory>(string[] args,
            string logName,
            Action<HostBuilderContext, IServiceCollection> configureServices = null)
            where TMessageHandlerSelector : class, IMessageHandlerSelector
            where TNetworkServer : class, INetworkServer
            where TNetworkSessionFactory : class, INetworkSessionFactory
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IMessageHandlerSelector, TMessageHandlerSelector>();
                    services.AddSingleton<INetworkServer, TNetworkServer>();
                    services.AddSingleton<INetworkSessionFactory, TNetworkSessionFactory>();
                    services.AddSingleton<IOpCodeToTypeConverter, OpCodeToTypeConverter>();
                    services.AddTransient<IDatabaseConnection, DatabaseConnection>();
                    services.AddSingleton<IStoredProcedureFactory, DapperStoredProcedureFactory>();
                    services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();
                    services.AddHostedService<NetworkServerWorker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "Solarponics";
                            config.SourceName = logName;
                        });

                    configureServices?.Invoke(hostContext, services);
                })
                .UseWindowsService()
                .UseSystemd();
            return host;
        }
    }
}