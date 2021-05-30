using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solarponics.Networking;
using Solarponics.Networking.Abstractions;
using Solarponics.ProvisioningServer.Data;
using Solarponics.ProvisioningServer.Domain;
using Solarponics.ProvisioningServer.Net;

namespace Solarponics.ProvisioningServer
{
    internal class Program : ProgramBase
    {
        public static void Main(string[] args)
        {
            var host =
                GenerateHostBuilder<ProvisioningMessageHandlerSelector, ProvisioningNetworkServer,
                    ProvisioningNetworkSessionFactory>(
                    args,
                    "Provisioning Server",
                    (context, services) =>
                    {
                        services.AddTransient<IProvisioningRepository, ProvisioningRepository>();
                    });
            host.Build().Run();
        }
    }
}