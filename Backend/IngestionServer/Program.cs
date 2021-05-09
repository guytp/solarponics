using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using Solarponics.IngestionServer.Abstractions;
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
                    var messageHandlerSelector = new MessageHandlerSelector();
                    services.AddSingleton<IMessageHandlerSelector>(messageHandlerSelector);
                    var opCodeToTypeConverter = new OpCodeToTypeConverter();
                    var sessionFactory = new NetworkSessionFactory(messageHandlerSelector, opCodeToTypeConverter);
                    services.AddSingleton<INetworkServer>(new NetworkServer(IPAddress.Any, 4201, sessionFactory));
                    services.AddSingleton<INetworkSessionFactory>(sessionFactory);
                    services.AddSingleton<IOpCodeToTypeConverter>(opCodeToTypeConverter);
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