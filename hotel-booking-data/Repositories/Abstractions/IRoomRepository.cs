using hotel_booking_models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Abstractions
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<List<Room>> GetAvailableRoomsByHotel(string hotelId);
        Room GetHotelRoom(string roomId);
    }
}
