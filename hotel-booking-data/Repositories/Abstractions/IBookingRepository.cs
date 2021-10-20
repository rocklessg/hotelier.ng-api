using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hotel_booking_dto;
using hotel_booking_models;
using hotel_booking_utilities;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        IQueryable<Booking> GetManagerBookings(string managerId);
        IQueryable<Booking> GetManagerBookingsSearchByHotel(string managerId, TransactionFilter filter);
        IQueryable<Booking> GetManagerBookingsFilterByDate(string managerId, TransactionFilter filter);
        IQueryable<Booking> GetManagerBookingsFilterByDate(string managerId, string year);
        IQueryable<Booking> GetManagerBookingsByHotelAndMonth(string managerId, TransactionFilter filter);      
    }
}
