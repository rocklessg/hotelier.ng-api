using hotel_booking_models;
using System.Linq;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        IQueryable<Booking> GetBookingsByCustomerId(string customerId);
    }
}
