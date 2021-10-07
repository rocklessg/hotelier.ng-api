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
        Task<List<Hotel>> GetAllAsync(Expression<Func<Hotel, bool>> expression = null, Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> orderby = null, List<string> Includes = null);
        Task<List<Hotel>> GetAllHotelsAsync();
        Hotel GetHotelById(string id);
        Task<List<Rating>> HotelRatings(string hotelId);
    }
}
