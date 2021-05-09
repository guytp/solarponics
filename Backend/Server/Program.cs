using System;
using System.Net;

namespace Solarponics.Server
{
    internal class Program
    {
        private static void Main()
        {
            var server = new CommandServer(IPAddress.Any, 4201);
            server.Start();

            Console.Write("Press enter to terminate server... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
            }

            Console.WriteLine("Server shutting down...");
            server.Stop();

            Console.WriteLine("Finished");
        }
    }
}