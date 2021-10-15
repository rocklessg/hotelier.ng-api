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
        public IQueryable<Booking> GetTransactionsByQuery(TransactionFilter filter)
        {
            var transactions =  GetAllTransactions();
            transactions = transactions.Where(transaction => transaction.Hotel.Name.Contains(filter.SearchQuery)
            && transaction.CreatedAt.Year.ToString() == (filter.Year))
            .OrderByDescending(booking => booking.CreatedAt);
            return transactions;
        }
        public IQueryable<Booking> GetTransactionsFilterByDate(TransactionFilter filter)
        {
            var bookings =  GetAllTransactions();
            bookings = bookings.Where(booking => booking.CreatedAt.Month.ToString() == (filter.Month)
            && booking.CreatedAt.Year.ToString() == (filter.Year))
            .OrderByDescending(booking => booking.CreatedAt);
            return bookings;
        }
        public IQueryable<Booking> GetTransactionsFilterByDate( string year)
        {
            var bookings = GetAllTransactions();
            bookings = bookings.Where(booking => booking.CreatedAt.Year.ToString() == (year))
            .OrderByDescending(booking => booking.CreatedAt);
            return bookings;
        }
        public IQueryable<Booking> GetTransactionsByHotelAndMonth(TransactionFilter filter)
        {
            var bookings = GetAllTransactions();
            bookings = bookings.Where(booking => booking.Hotel.Name.Contains(filter.SearchQuery)
            && booking.CreatedAt.Month.ToString() == (filter.Month)
            && booking.CreatedAt.Year.ToString() == (filter.Year))
            .OrderByDescending(booking => booking.CreatedAt);
            return bookings;
        }

        public IQueryable<Booking> GetAllTransactions() 
        {
            var transactions =   _context.Bookings
            .Include(x => x.Payment)
            .Include(x => x.Hotel)
            .Include(x => x.Customer)
            .Include(x => x.Customer.AppUser);
            return transactions;
        }
    }
}
