using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_api
{
    public static class Policies
    {
        /// <summary>
        /// Policy for Admin role
        /// </summary>
        public const string Admin = "Admin";
        /// <summary>
        /// Policy for HotelManager role
        /// </summary>
        public const string HotelManager = "HotelManager";
        /// <summary>
        /// Policy for a Regular User role
        /// </summary>
        public const string RegularUser = "RegularUser";
        /// <summary>
        /// Policy for an Admin and a Hotel Manager
        /// </summary>
        public const string AdminAndHotelManager = "AdminAndHotelManager";
        /// <summary>
        /// policy for a Hotel Manager and a Regular User
        /// </summary>
        public const string HotelManagerAndRegularUser = "HotelManagerAndRegularUser";

        /// <summary>
        /// Grants Admin User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }

        /// <summary>
        /// Grants Hotel Managers User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy HotelManagerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(HotelManager).Build();
        }

        /// <summary>
        /// Grants Regular Users User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy RegularUserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(RegularUser).Build();
        }

        /// <summary>
        /// Grants Admin and Hotel Managers User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy AdminAndHotelManagerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(AdminAndHotelManager).Build();
        }

        /// <summary>
        /// Grants Hotel Managers and Regular Users User Rights
        /// </summary>
        /// <returns></returns>
        public static AuthorizationPolicy HotelManagerAndRegularUserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(HotelManagerAndRegularUser).Build();
        }
    }
}
