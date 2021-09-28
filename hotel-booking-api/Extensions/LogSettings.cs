using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Serilog;
using Serilog.Events;
using System;
using System.Security.Cryptography.X509Certificates;

namespace hotel_booking_api.Extensions
{
    public static class LogSettings
    {
        public static void SetupSerilog(IConfiguration config)
        {
            var ravenDbSettings = config.GetSection("RavenDBConfigurations");
            //var ravenDBPassword = Environment.GetEnvironmentVariable("RavenDbPassword");
            var ravenDBPassword = "Coding@Edo123";

            DocumentStore ravenStore = new()
            {
                Urls = new string[] { ravenDbSettings.GetSection("ConnectionURL").Value },
                Database = ravenDbSettings.GetSection("DatabaseName").Value
            };

            ravenStore.Certificate = new X509Certificate2(ravenDbSettings.GetSection("CertificateFilePath").Value,
                ravenDBPassword, X509KeyStorageFlags.MachineKeySet);

            ravenStore.Initialize();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    path:  ".\\Logs\\log-.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information                    
                )
                .WriteTo.RavenDB(ravenStore)
                .CreateLogger();
        }
    }
}
