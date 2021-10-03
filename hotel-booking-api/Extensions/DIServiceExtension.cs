using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_core.Services;
using hotel_booking_utilities;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_booking_api.Extensions
{
    public static class DIServiceExtension
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            // Add Service Injections Here
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IHotelStatisticsService, HotelStatisticsService>();

            // Add Repository Injections Here

            // Add Fluent Validator Injections Here
        }
    }
}
