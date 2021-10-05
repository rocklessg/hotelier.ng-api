using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly HbaDbContext _context;

        public RoomRepository(HbaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetAvailableRoomsByHotel(string hotelId)
        {
            var rooms = await _context.Rooms
                .Include(room => room.Roomtype)
                .Where(room => room.Roomtype.Hotel.Id == hotelId)
                .Where(room => !room.IsBooked).ToListAsync();

            return rooms;
        }

        public Room GetHotelRoom(string roomId)
        {
            var getRoom = _context.Rooms.Include(x => x.Roomtype)
                    .Where(x => x.Id == roomId).FirstOrDefault();

            return getRoom;
        }
    }
}
