﻿using hotel_booking_data.Contexts;
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
    public class TransactionRepository : GenericRepository<Booking>, ITransactionRepository
    {
        private readonly HbaDbContext _context;

        public TransactionRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }
        
        public IQueryable<Booking> GetAllTransactions(TransactionFilter filter)
        {
            var bookings = _context.Bookings.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
            {
                bookings = bookings.Where(booking => booking.Hotel.Name.ToLower().Contains(filter.SearchQuery.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(filter.Month))
            {
                bookings = bookings.Where(booking => booking.CreatedAt.Month.ToString() == (filter.Month));
            }
            if (!string.IsNullOrWhiteSpace(filter.Year))
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
