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
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<Hotel> _dbSet;
        public HotelRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Hotel>();
        }

        public async Task<List<Hotel>> GetAllAsync(Expression<Func<Hotel, bool>> expression = null, Func<IQueryable<Hotel>, IOrderedQueryable<Hotel>> orderby = null, List<string> Includes = null)
        {
            var query = _dbSet.AsNoTracking();
            if (Includes != null) Includes.ForEach(x => query.Include(x));
            if (expression != null) query = query.Where(expression);
            if (orderby != null) query = orderby(query);
            return await query.ToListAsync();
        }

        public async Task<List<Room>> GetAvailableRoomsByHotel(string hotelId)
        {
            var hotel = _dbSet.Where(x => x.Id == hotelId).ToList();

            var rooms = await _context.Rooms
                .Include(room => room.Roomtype)
                .Where(room => room.Roomtype.Hotel.Id == hotelId)
                .Where(room => !room.IsBooked).ToListAsync();

            return rooms;
        }

        public Room GetHotelRoom(string roomId)
        {
            var hotel = _context.Rooms.FirstOrDefault(x => x.Id == roomId);

            var getRoom = _context.Rooms.Include(x => x.Roomtype)
                    .Where(x => x.Id == roomId).FirstOrDefault();

            return getRoom;
        }
    }
}
