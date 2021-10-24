using hotel_booking_dto;
using hotel_booking_models;
using System.Collections.Generic;
using System.Linq;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        IQueryable<Booking> GetManagerBookings(string managerId);
        IQueryable<Booking> GetManagerBookingsSearchByHotel(string managerId, TransactionFilter filter);
        IQueryable<Booking> GetManagerBookingsFilterByDate(string managerId, TransactionFilter filter);
        IQueryable<Booking> GetManagerBookingsFilterByDate(string managerId, string year);
        IQueryable<Booking> GetManagerBookingsByHotelAndMonth(string managerId, TransactionFilter filter);    
        IQueryable<Booking> GetBookingsByCustomerId(string customerId);
        IQueryable<Booking> GetBookingsByHotelId(string hotelId);
    }
}
