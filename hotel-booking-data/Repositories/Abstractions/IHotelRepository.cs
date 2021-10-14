using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        IQueryable<Hotel> GetAllHotels();
        Task<List<Hotel>> GetAllHotelsAsync();
        Hotel GetHotelById(string id);
        Task<List<Rating>> HotelRatings(string hotelId);
        Hotel GetHotelByIdForAddAmenity(string id);
        Task<Hotel> GetHotelsById(string hotelId);
        IQueryable<Hotel> GetHotelsByRating();
        IQueryable<Hotel> GetTopDeals();
    }
}
