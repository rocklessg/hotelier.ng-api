using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Repositories.Implementations
{
    public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<RoomType> _dbSet;

        public RoomTypeRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<RoomType>();
        }

        public async Task<List<RoomType>> GetAllAsync(
            Expression<Func<RoomType, bool>> expression = null,
            Func<IQueryable<RoomType>, IOrderedQueryable<RoomType>> orderby = null,
            List<string> Includes = null)
        {
            var query = _dbSet.AsNoTracking();
            if (Includes != null) Includes.ForEach(x => query = query.Include(x));
            if (expression != null) query = query.Where(expression);
            if (orderby != null) query = orderby(query);
            return await query.ToListAsync();
        }

        public async Task<List<RoomType>> GetRoomTypesInEachHotel(string hotelId)
        {
            var rooms = await _context.RoomTypes.Where(x => x.HotelId == hotelId).ToListAsync();

            return rooms;
        }



    }
}
