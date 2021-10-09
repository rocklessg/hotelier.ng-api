using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;

namespace hotel_booking_data.Repositories.Implementations
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly HbaDbContext _context;

        public BookingRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
