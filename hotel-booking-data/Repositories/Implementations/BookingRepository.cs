using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace hotel_booking_data.Repositories.Implementations
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly HbaDbContext _context;

        public BookingRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Booking> GetBookingsByCustomerId(string customerId)
        {
            var query = _context.Bookings.AsNoTracking()
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.Hotel)
                .Include(b => b.Payment)
                .Include(b => b.Room)
                .ThenInclude(r => r.Roomtype)
                .OrderBy(b => b.CreatedAt);
            return query;
        }
    }
}
