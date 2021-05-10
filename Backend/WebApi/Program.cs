using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Solarponics.WebApi.Exceptions;
#pragma warning disable 1591

namespace Solarponics.WebApi
{
    public class Program
    {
        private const string SslCertificateSerialNumberKey = "WebApi:SslCertificateSerialNumber";
        private const string SslCertificateFileConfigKey = "WebApi:SslCertificateFile";
        private const string SslCertificatePasswordConfigKey = "WebApi:SslCertificatePassword";

        public static async Task Main(string[] args)
        {
            var entryDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(entryDirectory);
            await CreateHostBuilder(args)
                .Build()
                .RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.ConfigureKestrel(
                                (hostingContext, serverOptions) =>
                                {
                                    var configuration = hostingContext.Configuration;
                                    var sslCertificateFilename =
                                        configuration.GetValue<string>(SslCertificateFileConfigKey);
                                    var sslCertificateSerialNumber =
                                        configuration.GetValue<string>(SslCertificateSerialNumberKey);
                                    var useSsl = !string.IsNullOrEmpty(sslCertificateSerialNumber) ||
                                                 !string.IsNullOrEmpty(sslCertificateFilename);

                                    if (useSsl)
                                    {
                                        X509Certificate2 certificate = null;
                                        if (!string.IsNullOrEmpty(sslCertificateFilename))
                                        {
                                            var rawCertificateData =
                                                File.ReadAllBytes(sslCertificateFilename);
                                            var certificatePassword =
                                                configuration.GetValue<string>(SslCertificatePasswordConfigKey);
                                            certificate = new X509Certificate2(rawCertificateData, certificatePassword);
                                        }
                                        else
                                        {
                                            var store = new X509Store(StoreLocation.LocalMachine);
                                            store.Open(OpenFlags.ReadOnly);
                                            var certificates = store.Certificates.Find(
                                                X509FindType.FindBySerialNumber,
                                                sslCertificateSerialNumber!,
                                                false);
                                            if (certificates.Count > 0)
                                                certificate = certificates[0];
                                        }

                                        if (certificate == null)
                                            throw new MissingSslCertificateException();

                                        serverOptions.ConfigureHttpsDefaults(
                                            httpsOptions => { httpsOptions.ServerCertificate = certificate; });
                                    }

                                    var port = configuration.GetValue<int>("WebApi:Port");
                                    serverOptions.Listen(
                                        IPAddress.Any,
                                        port,
                                        lo =>
                                        {
                                            if (useSsl) lo.UseHttps();
                                        });
                                })
                            .UseStartup(typeof(Startup));
                    }).UseWindowsService().UseSystemd();
        }
    }
}