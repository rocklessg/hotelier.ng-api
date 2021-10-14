using hotel_booking_data.Contexts;
using hotel_booking_data.Repositories.Abstractions;
using hotel_booking_models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        public IQueryable<Hotel> GetHotelsByRating()
        {
            var query = _dbSet.AsNoTracking();
            query = query.Include(x => x.Galleries)
                .Include(x => x.Ratings)
                .Include(x => x.RoomTypes)
                .OrderByDescending(h => h.Ratings.Sum(r => r.Ratings) / (double)h.Ratings.Count)
                .Take(5);
            return query;
        }
        public IQueryable<Hotel> GetTopDeals()
        {
            var query = _dbSet.AsNoTracking();
            query = query.Include(x => x.Galleries)
                .Include(x => x.Ratings)
                .Include(x => x.RoomTypes)
                .OrderBy(x => x.RoomTypes.OrderBy(rt => rt.Price).FirstOrDefault().Price)
                .Take(5);
            return query;
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
                         .Include(hotel => hotel.Galleries)
                         .Include(hotel => hotel.Ratings)
                         .Include(hotel => hotel.RoomTypes)
                         .Include(hotel => hotel.Amenities)
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
            return _context.Hotels.FirstOrDefault(x => x.Id == id);

        }

        public async Task<Hotel> GetHotelsById(string hotelId)
        {
            var hotel = await _context.Hotels.Where(x => x.Id == hotelId).FirstOrDefaultAsync();
            return hotel;
        }
    }
}
