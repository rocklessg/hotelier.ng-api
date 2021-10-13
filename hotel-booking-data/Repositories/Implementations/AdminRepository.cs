using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Booking>> GetAllTransactions() 
        {
            var transactions = await _context.Bookings.Include(x => x.Payment).Include(x => x.Hotel).Include(x => x.Customer).Include(x => x.Customer.AppUser).ToListAsync();
            return transactions;
        }


    }
}
