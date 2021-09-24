using Serilog;
using Serilog.Events;

namespace hotel_booking_api.Extensions
{
    public static class LogSettings
    {
        public static void SetupSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    path:  "/Logs/log-.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information
                ).CreateLogger();
        }
    }
}
