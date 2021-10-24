using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_dto;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public IQueryable<Booking> GetManagerBookings(string managerId)
        {
            var bookings = _context.Bookings
                .Where(x => x.Hotel.ManagerId == managerId)
                .Include(b => b.Payment)
                .Include(b => b.Customer)
                .Include(b => b.Hotel)
                .OrderByDescending(booking => booking.CreatedAt);
            return bookings;
        }
        public IQueryable<Booking> GetManagerBookingsSearchByHotel(string managerId, TransactionFilter filter)
        {
            var bookings = GetManagerBookings(managerId);
            bookings = bookings.Where(booking => booking.Hotel.Name.Contains(filter.SearchQuery)
            || booking.Hotel.Id.Contains(filter.SearchQuery)
            || booking.BookingReference.Contains(filter.SearchQuery)
            || booking.ServiceName.Contains(filter.SearchQuery)
            || booking.Payment.MethodOfPayment.Contains(filter.SearchQuery)
            || booking.Payment.Status.Contains(filter.SearchQuery)
            || booking.Payment.TransactionReference.Contains(filter.SearchQuery)
            || booking.Hotel.City.Contains(filter.SearchQuery)
            || booking.Hotel.Description.Contains(filter.SearchQuery)
            || booking.Hotel.State.Contains(filter.SearchQuery)
            || booking.Hotel.Phone.Contains(filter.SearchQuery)
            || booking.Hotel.Email.Contains(filter.SearchQuery)
            ).OrderByDescending(booking => booking.CreatedAt);
            return bookings;
        }
        public IQueryable<Booking> GetManagerBookingsFilterByDate(string managerId, TransactionFilter filter)
        {
            var bookings = GetManagerBookings(managerId);
            bookings = bookings.Where(booking => booking.CreatedAt.Month.ToString() == (filter.Month)
            && booking.CreatedAt.Year.ToString() == (filter.Year))
            .OrderByDescending(booking => booking.CreatedAt);
            return bookings;
        }
        public IQueryable<Booking> GetManagerBookingsFilterByDate(string managerId, string year)
        {
            var bookings = GetManagerBookings(managerId);
            bookings = bookings.Where(booking => booking.CreatedAt.Year.ToString() == (year))
            .OrderByDescending(booking => booking.CreatedAt);
            return bookings;
        }
        public IQueryable<Booking> GetManagerBookingsByHotelAndMonth(string managerId, TransactionFilter filter)
        {
            var bookings = GetManagerBookings(managerId);
            bookings = bookings.Where(booking => booking.HotelId == (filter.SearchQuery)
            && booking.CreatedAt.Month.ToString() == (filter.Month)
            && booking.CreatedAt.Year.ToString() == (filter.Year))
            .OrderByDescending(booking => booking.CreatedAt);
            return bookings;
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

        public IQueryable<Booking> GetBookingsByHotelId(string hotelId)
        {
            var query = _context.Bookings.AsNoTracking()
                .Where(b => b.HotelId == hotelId)
                .Where(b => b.PaymentStatus == true)
                .Include(b => b.Customer)
                .ThenInclude(c => c.AppUser)
                .Include(b => b.Payment)
                .OrderByDescending(b => b.Payment.Amount);
            return query;
        }
    }
}
