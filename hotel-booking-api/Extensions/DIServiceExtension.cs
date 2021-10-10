
﻿using FluentValidation;
using hotel_booking_core.Interface;

using hotel_booking_core.Interfaces;
using hotel_booking_core.Services;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_data.Repositories.Implementations;
using hotel_booking_data.UnitOfWork.Abstraction;
using hotel_booking_data.UnitOfWork.Implementation;
using hotel_booking_dto.AppUserDto;
using hotel_booking_dto.AuthenticationDtos;
using hotel_booking_dto.HotelDtos;
using hotel_booking_utilities;
using hotel_booking_utilities.Validators.AppUserValidator;
using hotel_booking_utilities.Validators.AuthenticationValidators;
using hotel_booking_utilities.Validators.HotelValidators;

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



            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAppUserService, AppUserService>();

            services.AddScoped<IAmenityService, AmenityService>();
            services.AddScoped<IHotelService, HotelService>();

            services.AddScoped<IHotelStatisticsService, HotelStatisticsService>();
            
            services.AddScoped<IManagerRepository, ManagerRepository>();
            



            // Add Repository Injections Here
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Add Model Services Injection Here

            services.AddScoped<IAmenityRepository, AmenityRepository>();
            // Add Fluent Validator Injections Here
            services.AddTransient<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();

            services.AddTransient<IValidator<UpdateHotelDto>, UpdateHotelDtoValidator>();
            services.AddTransient<IValidator<UpdatePasswordDto>, UpdatePasswordDtoValidator>();
            services.AddTransient<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();

            services.AddTransient<IValidator<UpdateAppUserDto>, UpdateAppUserDtoValidator>();

        }
    }
}
