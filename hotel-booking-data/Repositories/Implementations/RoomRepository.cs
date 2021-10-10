using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<Room> _dbSet;
        public RoomRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Room>();
        }

        public async Task<List<Room>> GetAllAsync(Expression<Func<Room, bool>> expression = null, Func<IQueryable<Room>, IOrderedQueryable<Room>> orderby = null, List<string> Includes = null)
        {
            var query = _dbSet.AsNoTracking();
            if (Includes != null) Includes.ForEach(x => query = query.Include(x));
            if (expression != null) query = query.Where(expression);
            if (orderby != null) query = orderby(query);
            return await query.ToListAsync();
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
