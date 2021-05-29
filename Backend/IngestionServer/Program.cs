using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solarponics.IngestionServer.Abstractions;
using Solarponics.IngestionServer.Data;
using Solarponics.IngestionServer.Domain;
using Solarponics.IngestionServer.Net;
using Solarponics.Networking;

namespace Solarponics.IngestionServer
{
    internal class Program : ProgramBase
    {
        public static void Main(string[] args)
        {
            var host =
                GenerateHostBuilder<IngestionMessageHandlerSelector, IngestionNetworkServer,
                    IngestionNetworkSessionFactory>(
                    args,
                    "Ingestion Server",
                    (context, services) => { services.AddTransient<ISensorRepository, SensorRepository>(); });
            host.Build().Run();
        }
    }
}