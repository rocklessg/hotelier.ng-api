using hotel_booking_models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hotel_booking_data.Contexts
{
    public class HbaDbContext : IdentityDbContext<User>
    {
        public HbaDbContext(DbContextOptions<HbaDbContext> options) : base(options)
        {

        }
    }
}
