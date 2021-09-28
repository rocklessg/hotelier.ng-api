using Microsoft.Extensions.DependencyInjection;

namespace hotel_booking_api
{
    public static class AuthorizationMiddleWare
    {
        public static void AddPolicyAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(configure =>
            {
                configure.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                configure.AddPolicy(Policies.HotelManager, Policies.HotelManagerPolicy());
                configure.AddPolicy(Policies.RegularUser, Policies.RegularUserPolicy());
                configure.AddPolicy(Policies.AdminAndHotelManager, Policies.AdminAndHotelManagerPolicy());
                configure.AddPolicy(Policies.HotelManagerAndRegularUser, Policies.HotelManagerAndRegularUserPolicy());
            });
        }
    }
}
