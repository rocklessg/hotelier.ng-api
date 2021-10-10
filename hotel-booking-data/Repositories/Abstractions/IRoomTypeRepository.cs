using hotel_booking_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IRoomTypeRepository : IGenericRepository<RoomType>
    {
        Task<List<RoomType>> GetAllAsync(
            Expression<Func<RoomType, bool>> expression = null, 
            Func<IQueryable<RoomType>, IOrderedQueryable<RoomType>> orderby = null, 
            List<string> Includes = null);
        Task<List<RoomType>> GetRoomTypesInEachHotel(string hotelId);

    }
}
