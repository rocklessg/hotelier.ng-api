using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<Payment> _dbSet;

        public PaymentRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Payment>();
        }

        public IQueryable<Payment> GetHotelTransactions(string hotelId)
        {
            var query = _dbSet.AsNoTracking();
            query = query.Include(payment => payment.Booking).ThenInclude(booking => booking.Hotel);
            query = query.Where(payment => payment.Booking.HotelId == hotelId);
            return query;
        }

        
    }
}
