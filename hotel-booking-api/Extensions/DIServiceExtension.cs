using FluentValidation;
using hotel_booking_core.Interface;
using hotel_booking_core.Interfaces;
using hotel_booking_core.Services;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_data.UnitOfWork.Implementation;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_utilities;
using hotel_booking_utilities.Validators.AuthenticationValidators;
using Microsoft.Extensions.DependencyInjection;

namespace hotel_booking_api.Extensions
{
    public static class DIServiceExtension
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            // Add Service Injections Here
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();

            // Add Repository Injections Here
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Model Services Injection Here
            services.AddScoped<IHotelService, HotelService>();

            // Add Fluent Validator Injections Here
            services.AddTransient<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();
        }
    }
}
