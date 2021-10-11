using FluentValidation.AspNetCore;
using hotel_booking_api.Extensions;
using hotel_booking_api.Middleware;
using hotel_booking_data.Contexts;
using hotel_booking_data.Seeder;
using hotel_booking_models;
using hotel_booking_models.Cloudinary;
using hotel_booking_models.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace hotel_booking_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            StaticConfig = configuration;
            Environment = environment;
        }

        public static IConfiguration StaticConfig { get; private set; }
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextAndConfigurations(Environment, Configuration);

            // Configure Mailing Service
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            

            // Adds our Authorization Policies to the Dependecy Injection Container
            services.AddPolicyAuthorization();

            
            services.AddAuthentication();

            // Configure Identity
            services.ConfigureIdentity();
            // Add Jwt Authentication and Authorization
            services.ConfigureAuthentication(Configuration);

            // Configure AutoMapper
            services.ConfigureAutoMappers();

            // Configure Cloudinary
            services.Configure<ImageUploadSettings>(Configuration.GetSection("CloudSettings"));
            services.AddCloudinary(CloudinaryServiceExtension.GetAccount(Configuration));

            services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling 
            = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers()
                .AddNewtonsoftJson(op => op.SerializerSettings
                    .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddMvc().AddFluentValidation(fv => {
                fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                fv.ImplicitlyValidateChildProperties = true;
            });

            services.AddSwagger();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            // Register Dependency Injection Service Extension
            services.AddDependencyInjection(); 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            HbaDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Management Api v1"));

            HbaSeeder.SeedData(dbContext, userManager, roleManager).GetAwaiter().GetResult();

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
