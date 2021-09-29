using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace hotel_booking_api.Extensions
{
    public static class AuthenticationServiceExtension
    {
        /// <summary>
        /// Configures JWT Authentication and Sets up Policy Based Authorization Services.
        /// Returns Services of type IServiceCollection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateAudience = true,
                   ValidateIssuer = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidAudience = Startup.StaticConfig["JwtSettings:Audience"],
                   ValidIssuer = Startup.StaticConfig["JwtSettings:Issuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding
                   .UTF8.GetBytes(Startup.StaticConfig["JwtSettings:SecretKey"])),
                   ClockSkew = TimeSpan.Zero
               };
           });
        }
    }
}
