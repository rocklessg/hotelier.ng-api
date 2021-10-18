using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class AdminRepository : GenericRepository<AppUser>, IAdminRepository
    {
        private readonly HbaDbContext _context;

        public AdminRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }
        
        public IQueryable<Booking> GetAllTransactions(TransactionFilter filter)
        {
            var bookings = _context.Bookings.AsQueryable();
     
            if (filter.SearchQuery != null)
            {
                bookings = bookings.Where(booking => booking.Hotel.Name.ToLower().Contains(filter.SearchQuery.ToLower()));
            }
            if ( filter.Month != null)
            {
                bookings = bookings.Where(booking => booking.CreatedAt.Month.ToString() == (filter.Month));
            }
             if (filter.Year != null)
            {
                bookings = bookings.Where(booking => booking.CreatedAt.Year.ToString() == (filter.Year));              
            }
            bookings = bookings.Include(x => x.Payment)
            .Include(x => x.Hotel)
            .Include(x => x.Customer)
            .Include(x => x.Customer.AppUser).OrderByDescending(booking => booking.CreatedAt);

            return bookings;
        }

        public async Task<Manager> GetManagerById(string id)
        {
            return await _context.Managers.FirstOrDefaultAsync(x => x.AppUserId == id);
        }

    }
}
