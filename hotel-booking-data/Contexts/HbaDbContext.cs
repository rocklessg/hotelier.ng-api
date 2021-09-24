using hotel_booking_models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hotel_booking_data.Contexts
{
    public class HbaDbContext : IdentityDbContext<AppUser>
    {
        public HbaDbContext(DbContextOptions<HbaDbContext> options) : base(options)
        {

        }

        public DbSet<Amenity> Amenities {  get; set; }
        public DbSet<Booking> Bookings {  get; set; }
        public DbSet<Customer> Customers {  get; set; }
        public DbSet<Hotel> Hotels {  get; set; }
        public DbSet<Manager> Managers {  get; set; }
        public DbSet<Payment> Payments {  get; set; }
        public DbSet<Rating> Ratings {  get; set; }
        public DbSet<Review> Reviews {  get; set; }
        public DbSet<Room> Rooms {  get; set; }
        public DbSet<RoomType> RoomTypes {  get; set; }
        public DbSet<WishList> WishLists {  get; set; }
    }
}
