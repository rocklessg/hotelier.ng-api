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
            if (Includes != null) Includes.ForEach(x => query = query.Include(x));
            if (expression != null) query = query.Where(expression);
            if (orderby != null) query = orderby(query);
            return await query.ToListAsync();
        }

        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            var hotelList = _dbSet
               .Include(c => c.Galleries)
               .Include(c => c.Ratings)
               .Include(c => c.RoomTypes);
            return await hotelList.ToListAsync();
        }


        public Hotel GetHotelById(string id)
        {
            var hotel = _dbSet.Where(hotel => hotel.Id == id)
                         //.Include(hotel => hotel.Amenities)
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

        public Hotel GetHotelByIdForAddAmenity(string id)
        {
           return  _context.Hotels.FirstOrDefault(x => x.Id == id);
            
        }

        public async Task<Hotel> GetHotelsById(string hotelId) 
        {
            var hotel = await _context.Hotels.Where(x => x.Id == hotelId).FirstOrDefaultAsync();
            return hotel;
        }
    }
}
