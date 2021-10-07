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
    public class HotelRepository : GenericRepository<RoomType>, IHotelRepository
    {
        private readonly HbaDbContext _context;
        private readonly DbSet<RoomType> _dbSet;
        public HotelRepository(HbaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<RoomType>();
        }
        public async Task<List<RoomType>> GetAllAsync(Expression<Func<RoomType, bool>> expression = null, Func<IQueryable<RoomType>, IOrderedQueryable<RoomType>> orderby = null, List<string> Includes = null)
        {
            var query = _dbSet.AsNoTracking();
            if (Includes != null) Includes.ForEach(x => query = query.Include(x));
            if (expression != null) query = query.Where(expression);
            if (orderby != null) query = orderby(query);
            return await query.ToListAsync();
        }

        public async Task<List<RoomType>> GetAllHotelsAsync()
        {
            var hotelList = _dbSet
               .Include(c => c.Galleries)
               .Include(c => c.Ratings)
               .Include(c => c.RoomTypes);
            return await hotelList.ToListAsync();
        }


        public RoomType GetHotelById(string id)
        {
            var hotel = _dbSet.Where(hotel => hotel.Id == id)
                         .Include(hotel => hotel.Galleries)
                         .Include(hotel => hotel.Ratings)
                         .Include(hotel => hotel.RoomTypes)
                         .Include(hotel => hotel.Reviews)
                         .ThenInclude(review => review.Customer.AppUser);
            return hotel.FirstOrDefault();
        }

        public async Task<List<Rating>> HotelRatings(string hotelId)
        {
            var ratings = await _context.Ratings
                    .Where(x => x.HotelId == hotelId).ToListAsync();

            return ratings;
        }
    }
}
