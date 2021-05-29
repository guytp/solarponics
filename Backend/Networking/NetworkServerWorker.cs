using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Solarponics.Networking.Abstractions;

namespace Solarponics.Networking
{
    public class NetworkServerWorker : BackgroundService
    {
        private readonly INetworkServer _networkServer;

        public NetworkServerWorker(INetworkServer networkServer)
        {
            _networkServer = networkServer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting server...");
            _networkServer.Start();
            Console.WriteLine("Server started");

            while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);

            Console.WriteLine("Server shutting down...");
            _networkServer.Stop();

            Console.WriteLine("Server finished");
        }
    }
}