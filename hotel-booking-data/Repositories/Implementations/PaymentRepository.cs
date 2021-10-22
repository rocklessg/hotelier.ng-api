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

        public PaymentRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Payment> GetHotelTransactions(string hotelId)
        {
            var query = _context.Payments
                .Include(payment => payment.Booking)
                .ThenInclude(x => x.Hotel)
                .Where(x => x.Booking.HotelId == hotelId);
            return query;
        }

        
    }
}
